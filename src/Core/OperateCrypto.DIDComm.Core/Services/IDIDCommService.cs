using OperateCrypto.DIDComm.Core.Models;

namespace OperateCrypto.DIDComm.Core.Services;

/// <summary>
/// Core DIDComm service interface for packing and unpacking messages
/// </summary>
public interface IDIDCommService
{
    /// <summary>
    /// Packs a DIDComm message into an encrypted envelope
    /// </summary>
    /// <param name="message">The plaintext DIDComm message</param>
    /// <param name="toDid">Recipient DID</param>
    /// <param name="fromDid">Optional sender DID (for authenticated encryption)</param>
    /// <returns>Encrypted DIDComm envelope</returns>
    Task<DIDCommEnvelope> PackMessageAsync(DIDCommMessage message, string toDid, string? fromDid = null);

    /// <summary>
    /// Unpacks an encrypted DIDComm envelope into a plaintext message
    /// </summary>
    /// <param name="envelope">The encrypted envelope</param>
    /// <param name="recipientDid">The recipient DID (for key lookup)</param>
    /// <returns>Decrypted DIDComm message</returns>
    Task<DIDCommMessage> UnpackMessageAsync(DIDCommEnvelope envelope, string recipientDid);

    /// <summary>
    /// Signs a DIDComm message (for authenticated messages)
    /// </summary>
    /// <param name="message">The message to sign</param>
    /// <param name="signerDid">The DID performing the signature</param>
    /// <returns>JWS signed message string</returns>
    Task<string> SignMessageAsync(DIDCommMessage message, string signerDid);

    /// <summary>
    /// Verifies a signed DIDComm message
    /// </summary>
    /// <param name="signedMessage">The JWS signed message</param>
    /// <returns>True if signature is valid</returns>
    Task<bool> VerifyMessageAsync(string signedMessage);

    /// <summary>
    /// Validates a DIDComm message structure
    /// </summary>
    /// <param name="message">Message to validate</param>
    /// <returns>True if message is valid</returns>
    bool ValidateMessage(DIDCommMessage message);
}
