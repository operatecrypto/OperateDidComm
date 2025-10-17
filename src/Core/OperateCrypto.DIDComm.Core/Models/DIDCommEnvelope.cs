namespace OperateCrypto.DIDComm.Core.Models;

/// <summary>
/// Encrypted DIDComm message envelope (JWE format)
/// </summary>
public class DIDCommEnvelope
{
    /// <summary>
    /// Protected header (Base64URL encoded)
    /// </summary>
    public string Protected { get; set; } = string.Empty;

    /// <summary>
    /// Initialization vector for encryption
    /// </summary>
    public string Iv { get; set; } = string.Empty;

    /// <summary>
    /// Encrypted ciphertext
    /// </summary>
    public string Ciphertext { get; set; } = string.Empty;

    /// <summary>
    /// Authentication tag
    /// </summary>
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// Recipients (for multiple recipient support)
    /// </summary>
    public List<Recipient> Recipients { get; set; } = new();
}

/// <summary>
/// Recipient information in JWE envelope
/// </summary>
public class Recipient
{
    /// <summary>
    /// Encrypted key for this recipient
    /// </summary>
    public string EncryptedKey { get; set; } = string.Empty;

    /// <summary>
    /// Recipient-specific header
    /// </summary>
    public RecipientHeader? Header { get; set; }
}

/// <summary>
/// Header information for recipient
/// </summary>
public class RecipientHeader
{
    /// <summary>
    /// Key identifier
    /// </summary>
    public string? Kid { get; set; }

    /// <summary>
    /// Algorithm used
    /// </summary>
    public string? Alg { get; set; }

    /// <summary>
    /// Ephemeral public key
    /// </summary>
    public object? Epk { get; set; }

    /// <summary>
    /// Agreement PartyUInfo
    /// </summary>
    public string? Apu { get; set; }

    /// <summary>
    /// Agreement PartyVInfo
    /// </summary>
    public string? Apv { get; set; }
}
