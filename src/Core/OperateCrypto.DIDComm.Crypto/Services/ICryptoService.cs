using OperateCrypto.DIDComm.Crypto.Models;

namespace OperateCrypto.DIDComm.Crypto.Services;

/// <summary>
/// Cryptographic service interface for DIDComm operations
/// </summary>
public interface ICryptoService
{
    /// <summary>
    /// Generates a new key pair
    /// </summary>
    /// <param name="type">Type of key to generate</param>
    /// <returns>Generated key pair</returns>
    Task<KeyPair> GenerateKeyPairAsync(KeyType type);

    /// <summary>
    /// Encrypts data for a recipient using their public key
    /// </summary>
    /// <param name="data">Data to encrypt</param>
    /// <param name="recipientPublicKey">Recipient's public key</param>
    /// <param name="senderPrivateKey">Optional sender's private key for authenticated encryption</param>
    /// <returns>Encrypted data</returns>
    Task<byte[]> EncryptAsync(byte[] data, string recipientPublicKey, string? senderPrivateKey = null);

    /// <summary>
    /// Decrypts data using private key
    /// </summary>
    /// <param name="encryptedData">Encrypted data</param>
    /// <param name="recipientPrivateKey">Recipient's private key</param>
    /// <param name="senderPublicKey">Optional sender's public key for authenticated encryption</param>
    /// <returns>Decrypted data</returns>
    Task<byte[]> DecryptAsync(byte[] encryptedData, string recipientPrivateKey, string? senderPublicKey = null);

    /// <summary>
    /// Signs data with private key
    /// </summary>
    /// <param name="data">Data to sign</param>
    /// <param name="privateKey">Private key for signing</param>
    /// <returns>Signature (Base64 encoded)</returns>
    Task<string> SignAsync(byte[] data, string privateKey);

    /// <summary>
    /// Verifies signature
    /// </summary>
    /// <param name="data">Original data</param>
    /// <param name="signature">Signature to verify</param>
    /// <param name="publicKey">Public key</param>
    /// <returns>True if signature is valid</returns>
    Task<bool> VerifyAsync(byte[] data, string signature, string publicKey);

    /// <summary>
    /// Performs Elliptic Curve Diffie-Hellman key agreement
    /// </summary>
    /// <param name="privateKey">Own private key</param>
    /// <param name="publicKey">Other party's public key</param>
    /// <returns>Shared secret</returns>
    Task<byte[]> ECDHAsync(string privateKey, string publicKey);

    /// <summary>
    /// Derives a key from password (for key encryption)
    /// </summary>
    /// <param name="password">Password</param>
    /// <param name="salt">Salt</param>
    /// <param name="iterations">PBKDF2 iterations</param>
    /// <returns>Derived key</returns>
    Task<byte[]> DeriveKeyAsync(string password, byte[] salt, int iterations = 10000);

    /// <summary>
    /// Encrypts a private key for storage
    /// </summary>
    /// <param name="privateKey">Private key to encrypt</param>
    /// <param name="masterKey">Master encryption key</param>
    /// <returns>Encrypted private key</returns>
    Task<string> EncryptPrivateKeyAsync(string privateKey, string masterKey);

    /// <summary>
    /// Decrypts a private key from storage
    /// </summary>
    /// <param name="encryptedPrivateKey">Encrypted private key</param>
    /// <param name="masterKey">Master encryption key</param>
    /// <returns>Decrypted private key</returns>
    Task<string> DecryptPrivateKeyAsync(string encryptedPrivateKey, string masterKey);
}
