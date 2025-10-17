using OperateCrypto.DIDComm.Core.Models;

namespace OperateCrypto.DIDComm.Handlers.Handlers;

/// <summary>
/// Handler for DIDComm Basic Message protocol
/// https://didcomm.org/basicmessage/2.0/
/// </summary>
public class BasicMessageHandler : IMessageHandler
{
    private const string BASIC_MESSAGE_TYPE = "https://didcomm.org/basicmessage/2.0/message";

    public bool CanHandle(string messageType)
    {
        return messageType == BASIC_MESSAGE_TYPE;
    }

    public async Task<DIDCommMessage?> HandleMessageAsync(DIDCommMessage message)
    {
        // Basic message handling
        // 1. Log the message
        Console.WriteLine($"[BasicMessageHandler] Received message from {message.From}");
        Console.WriteLine($"[BasicMessageHandler] Message: {message.Body}");

        // 2. Store in database (would be injected via repository)
        // await _messageRepository.AddAsync(message);

        // 3. Trigger any notifications
        // await _notificationService.NotifyAsync(message.To, message);

        await Task.CompletedTask;

        // Basic messages typically don't require a response
        // unless the sender explicitly requests one
        return null;
    }

    public List<string> SupportedMessageTypes()
    {
        return new List<string> { BASIC_MESSAGE_TYPE };
    }
}
