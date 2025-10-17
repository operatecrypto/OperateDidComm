using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperateCrypto.DIDComm.Api.Models;
using OperateCrypto.DIDComm.Core.Models;
using OperateCrypto.DIDComm.Core.Services;
using OperateCrypto.DIDComm.Data.Repositories;
using OperateCrypto.DIDComm.Handlers;
using System.Text.Json;

namespace OperateCrypto.DIDComm.Api.Controllers;

/// <summary>
/// DIDComm messaging API controller
/// Handles sending and receiving DIDComm v2 messages
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DIDCommController : ControllerBase
{
    private readonly IDIDCommService _didCommService;
    private readonly IMessageRepository _messageRepository;
    private readonly IEnumerable<IMessageHandler> _messageHandlers;
    private readonly ILogger<DIDCommController> _logger;

    public DIDCommController(
        IDIDCommService didCommService,
        IMessageRepository messageRepository,
        IEnumerable<IMessageHandler> messageHandlers,
        ILogger<DIDCommController> logger)
    {
        _didCommService = didCommService;
        _messageRepository = messageRepository;
        _messageHandlers = messageHandlers;
        _logger = logger;
    }

    /// <summary>
    /// Send a DIDComm message
    /// </summary>
    /// <param name="request">Message details</param>
    /// <returns>Send operation result</returns>
    [HttpPost("send")]
    [AllowAnonymous] // Temporary: Allow testing without auth
    [ProducesResponseType(typeof(SendMessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        try
        {
            // 1. Validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2. Create DIDComm message
            var message = new DIDCommMessage
            {
                From = request.From ?? User.Identity?.Name,
                To = request.To,
                Type = request.Type ?? "https://didcomm.org/basicmessage/2.0/message",
                Body = request.Body,
                ThreadId = request.ThreadId
            };

            // 3. Validate message structure
            if (!_didCommService.ValidateMessage(message))
            {
                return BadRequest("Invalid DIDComm message structure");
            }

            // 4. Pack and encrypt message (TODO: implement actual packing)
            // var envelope = await _didCommService.PackMessageAsync(message, request.To, request.From);

            // 5. Send to recipient endpoint (TODO: implement HTTP delivery)
            // var recipientEndpoint = await GetRecipientEndpoint(request.To);
            // await DeliverMessage(recipientEndpoint, envelope);

            // 6. Store message in database
            var messageRecord = new Data.Entities.MessageRecord
            {
                MessageId = message.Id,
                From = message.From ?? string.Empty,
                To = message.To,
                Type = message.Type,
                Body = JsonSerializer.Serialize(message.Body),
                ThreadId = message.ThreadId,
                CreatedTime = message.CreatedTime,
                Status = "sent",
                Direction = "out",
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(messageRecord);

            _logger.LogInformation("Message {MessageId} sent from {From} to {To}",
                message.Id, message.From, message.To);

            // 7. Return success response
            return Ok(new SendMessageResponse
            {
                MessageId = message.Id,
                Status = "sent",
                SentAt = DateTime.UtcNow,
                Success = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message");
            return StatusCode(500, new SendMessageResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    /// <summary>
    /// Receive a DIDComm message (endpoint for other DIDs to send messages)
    /// </summary>
    /// <param name="envelope">Encrypted DIDComm envelope</param>
    /// <returns>Acknowledgment or response message</returns>
    [HttpPost("receive")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReceiveMessage([FromBody] DIDCommEnvelope envelope)
    {
        try
        {
            // 1. Unpack and decrypt message (TODO: implement actual unpacking)
            // var message = await _didCommService.UnpackMessageAsync(envelope, recipientDid);

            // For now, create a placeholder message
            var message = new DIDCommMessage
            {
                Id = Guid.NewGuid().ToString(),
                Type = "https://didcomm.org/basicmessage/2.0/message",
                From = "unknown",
                To = "unknown",
                Body = new { }
            };

            // 2. Verify signature if present
            // await _didCommService.VerifyMessageAsync(signedMessage);

            // 3. Store in database
            var messageRecord = new Data.Entities.MessageRecord
            {
                MessageId = message.Id,
                From = message.From ?? string.Empty,
                To = message.To,
                Type = message.Type,
                Body = JsonSerializer.Serialize(message.Body),
                ThreadId = message.ThreadId,
                CreatedTime = message.CreatedTime,
                Status = "received",
                Direction = "in",
                ReceivedAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(messageRecord);

            // 4. Find and invoke appropriate message handler
            var handler = _messageHandlers.FirstOrDefault(h => h.CanHandle(message.Type));
            DIDCommMessage? response = null;

            if (handler != null)
            {
                response = await handler.HandleMessageAsync(message);
                _logger.LogInformation("Message {MessageId} handled by {HandlerType}",
                    message.Id, handler.GetType().Name);
            }
            else
            {
                _logger.LogWarning("No handler found for message type {MessageType}", message.Type);
            }

            // 5. Return acknowledgment or response
            if (response != null)
            {
                return Ok(response);
            }

            return Ok(new { status = "received", messageId = message.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error receiving message");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get messages for authenticated user
    /// </summary>
    /// <param name="did">DID to get messages for</param>
    /// <param name="unreadOnly">Filter for unread messages only</param>
    /// <param name="skip">Pagination skip</param>
    /// <param name="take">Pagination take</param>
    /// <returns>List of messages</returns>
    [HttpGet("messages")]
    [AllowAnonymous] // Temporary: Allow testing without auth
    [ProducesResponseType(typeof(List<Data.Entities.MessageRecord>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessages(
        [FromQuery] string? did,
        [FromQuery] bool unreadOnly = false,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        try
        {
            var userDid = did ?? User.Identity?.Name;

            if (string.IsNullOrEmpty(userDid))
                return BadRequest("DID is required");

            var messages = await _messageRepository.GetMessagesByDidAsync(userDid, skip, take);

            if (unreadOnly)
            {
                messages = messages.Where(m => m.Status != "read").ToList();
            }

            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving messages");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Mark a message as read
    /// </summary>
    /// <param name="messageId">Message ID to mark as read</param>
    /// <returns>Success status</returns>
    [HttpPut("messages/{messageId}/read")]
    [AllowAnonymous] // Temporary: Allow testing without auth
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(Guid messageId)
    {
        try
        {
            var result = await _messageRepository.MarkAsReadAsync(messageId);

            if (!result)
                return NotFound(new { error = "Message not found" });

            return Ok(new { status = "marked as read" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking message as read");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
