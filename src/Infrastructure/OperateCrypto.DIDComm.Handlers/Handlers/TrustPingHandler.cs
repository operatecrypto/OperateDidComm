using OperateCrypto.DIDComm.Core.Models;

namespace OperateCrypto.DIDComm.Handlers.Handlers;

/// <summary>
/// Handler for DIDComm Trust Ping protocol
/// https://didcomm.org/trust-ping/2.0/
/// Used to test connectivity and verify that a DID can be reached
/// </summary>
public class TrustPingHandler : IMessageHandler
{
    private const string PING_TYPE = "https://didcomm.org/trust-ping/2.0/ping";
    private const string PING_RESPONSE_TYPE = "https://didcomm.org/trust-ping/2.0/ping-response";

    public bool CanHandle(string messageType)
    {
        return messageType == PING_TYPE || messageType == PING_RESPONSE_TYPE;
    }

    public async Task<DIDCommMessage?> HandleMessageAsync(DIDCommMessage message)
    {
        await Task.CompletedTask;

        if (message.Type == PING_TYPE)
        {
            // Received a ping, send a ping response
            Console.WriteLine($"[TrustPingHandler] Received ping from {message.From}");

            var response = new DIDCommMessage
            {
                Id = Guid.NewGuid().ToString(),
                Type = PING_RESPONSE_TYPE,
                From = message.To, // Swap from/to for response
                To = message.From,
                ThreadId = message.ThreadId ?? message.Id, // Use original message ID as thread
                Body = new
                {
                    comment = "Ping response",
                    timestamp = DateTime.UtcNow
                }
            };

            Console.WriteLine($"[TrustPingHandler] Sending ping response to {response.To}");
            return response;
        }
        else if (message.Type == PING_RESPONSE_TYPE)
        {
            // Received a ping response
            Console.WriteLine($"[TrustPingHandler] Received ping response from {message.From}");

            // Log the round-trip time or update connection status
            // No response needed for a ping-response
            return null;
        }

        return null;
    }

    public List<string> SupportedMessageTypes()
    {
        return new List<string> { PING_TYPE, PING_RESPONSE_TYPE };
    }
}
