namespace OperateCrypto.DIDComm.Data.Entities;

/// <summary>
/// Database entity for DIDComm cryptographic keys
/// Stores encrypted private keys for DIDs
/// </summary>
public class DIDCommKey
{
    /// <summary>
    /// Primary key - surrogate ID
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// DID that owns this key
    /// </summary>
    public string Did { get; set; } = string.Empty;

    /// <summary>
    /// Key identifier (e.g., "did:web:alice.operatedid.com#key-1")
    /// </summary>
    public string KeyId { get; set; } = string.Empty;

    /// <summary>
    /// Key type (Ed25519, X25519, Secp256k1, P256, RSA)
    /// </summary>
    public string KeyType { get; set; } = string.Empty;

    /// <summary>
    /// Public key (Base64 or JWK format)
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>
    /// Private key (encrypted with master key)
    /// </summary>
    public string? PrivateKey { get; set; }

    /// <summary>
    /// Key purpose (signing, keyAgreement, authentication)
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// Whether key is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When key was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When key was revoked
    /// </summary>
    public DateTime? RevokedAt { get; set; }

    // Standard system fields
    public bool Deleted { get; set; } = false;
    public bool Archived { get; set; } = false;
    public DateTime LastModifiedOn { get; set; } = DateTime.UtcNow;
    public int? LastModifiedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? SourceAppID { get; set; }
    public int? ClientAccountID { get; set; }
    public int? AppDomainID { get; set; }
    public int? DataDomainID { get; set; }
    public int? DataSegmentID { get; set; }
    public Guid ResID { get; set; } = Guid.NewGuid();
}
