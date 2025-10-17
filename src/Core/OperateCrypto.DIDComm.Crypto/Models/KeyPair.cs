namespace OperateCrypto.DIDComm.Crypto.Models;

/// <summary>
/// Represents a cryptographic key pair
/// </summary>
public class KeyPair
{
    /// <summary>
    /// Public key (Base64 or JWK format)
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>
    /// Private key (Base64 or JWK format) - should be kept secure
    /// </summary>
    public string PrivateKey { get; set; } = string.Empty;

    /// <summary>
    /// Type of key
    /// </summary>
    public KeyType Type { get; set; }

    /// <summary>
    /// Key identifier
    /// </summary>
    public string KeyId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Supported key types for DIDComm
/// </summary>
public enum KeyType
{
    /// <summary>
    /// Ed25519 - EdDSA signature with Curve25519
    /// </summary>
    Ed25519,

    /// <summary>
    /// X25519 - ECDH key agreement with Curve25519
    /// </summary>
    X25519,

    /// <summary>
    /// Secp256k1 - Bitcoin/Ethereum curve
    /// </summary>
    Secp256k1,

    /// <summary>
    /// P-256 (secp256r1) - NIST curve
    /// </summary>
    P256,

    /// <summary>
    /// RSA for legacy support
    /// </summary>
    RSA
}
