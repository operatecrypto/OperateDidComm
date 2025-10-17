using System.ComponentModel.DataAnnotations;

namespace OperateCrypto.DIDComm.Api.Models;

/// <summary>
/// Request model for sending a DIDComm message
/// </summary>
public class SendMessageRequest
{
    /// <summary>
    /// Sender DID (optional - can be inferred from authentication)
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// Recipient DID (required)
    /// </summary>
    [Required]
    public string To { get; set; } = string.Empty;

    /// <summary>
    /// Message type URI (defaults to basic message)
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Message body content (JSON object)
    /// </summary>
    [Required]
    public object Body { get; set; } = new();

    /// <summary>
    /// Optional thread ID for conversation threading
    /// </summary>
    public string? ThreadId { get; set; }

    /// <summary>
    /// Optional attachments
    /// </summary>
    public List<AttachmentDto>? Attachments { get; set; }
}

/// <summary>
/// Attachment DTO
/// </summary>
public class AttachmentDto
{
    public string? Description { get; set; }
    public string? Filename { get; set; }
    public string? MediaType { get; set; }
    public string? Base64Data { get; set; }
}
