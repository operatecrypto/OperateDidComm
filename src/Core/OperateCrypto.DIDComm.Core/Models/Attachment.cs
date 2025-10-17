namespace OperateCrypto.DIDComm.Core.Models;

/// <summary>
/// Represents a DIDComm message attachment
/// </summary>
public class Attachment
{
    /// <summary>
    /// Attachment identifier
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Optional description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Filename if applicable
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Media type (MIME type)
    /// </summary>
    public string? MediaType { get; set; }

    /// <summary>
    /// Format of the data
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Last modified timestamp
    /// </summary>
    public DateTime? LastModTime { get; set; }

    /// <summary>
    /// Byte count
    /// </summary>
    public long? ByteCount { get; set; }

    /// <summary>
    /// Attachment data
    /// </summary>
    public AttachmentData? Data { get; set; }
}

/// <summary>
/// Attachment data container
/// </summary>
public class AttachmentData
{
    /// <summary>
    /// Base64 encoded data
    /// </summary>
    public string? Base64 { get; set; }

    /// <summary>
    /// JSON object data
    /// </summary>
    public object? Json { get; set; }

    /// <summary>
    /// Links to external data
    /// </summary>
    public List<string>? Links { get; set; }

    /// <summary>
    /// JWS (JSON Web Signature) signed data
    /// </summary>
    public string? Jws { get; set; }

    /// <summary>
    /// SHA-256 hash of the data
    /// </summary>
    public string? Sha256 { get; set; }
}
