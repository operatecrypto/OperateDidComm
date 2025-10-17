namespace OperateCrypto.DIDComm.Core.Models;

/// <summary>
/// Represents a DIDComm v2 message structure
/// </summary>
public class DIDCommMessage
{
    /// <summary>
    /// Unique message identifier
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Message type URI (e.g., "https://didcomm.org/basicmessage/2.0/message")
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Sender DID
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// Recipient DID (single recipient for now)
    /// </summary>
    public string To { get; set; } = string.Empty;

    /// <summary>
    /// Unix timestamp in milliseconds
    /// </summary>
    public long CreatedTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    /// <summary>
    /// Optional expiration time
    /// </summary>
    public DateTime? ExpiresTime { get; set; }

    /// <summary>
    /// Message body content (JSON serializable)
    /// </summary>
    public object? Body { get; set; }

    /// <summary>
    /// Optional attachments
    /// </summary>
    public List<Attachment>? Attachments { get; set; }

    /// <summary>
    /// Thread identifier for message threading
    /// </summary>
    public string? ThreadId { get; set; }

    /// <summary>
    /// Parent thread identifier for nested conversations
    /// </summary>
    public string? ParentThreadId { get; set; }

    /// <summary>
    /// Optional custom headers
    /// </summary>
    public Dictionary<string, object>? Headers { get; set; }
}
