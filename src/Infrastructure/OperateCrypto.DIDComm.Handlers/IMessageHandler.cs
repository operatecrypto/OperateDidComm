using OperateCrypto.DIDComm.Core.Models;

namespace OperateCrypto.DIDComm.Handlers;

/// <summary>
/// Interface for DIDComm message handlers
/// Each handler processes a specific DIDComm message type
/// </summary>
public interface IMessageHandler
{
    /// <summary>
    /// Checks if this handler can process the given message type
    /// </summary>
    /// <param name="messageType">The message type URI</param>
    /// <returns>True if handler can process this message type</returns>
    bool CanHandle(string messageType);

    /// <summary>
    /// Handles the DIDComm message
    /// </summary>
    /// <param name="message">The received DIDComm message</param>
    /// <returns>Optional response message, or null if no response needed</returns>
    Task<DIDCommMessage?> HandleMessageAsync(DIDCommMessage message);

    /// <summary>
    /// Gets the message types this handler supports
    /// </summary>
    /// <returns>List of supported message type URIs</returns>
    List<string> SupportedMessageTypes();
}
