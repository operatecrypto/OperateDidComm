namespace OperateCrypto.DIDComm.Data.Entities;

/// <summary>
/// Database entity for DIDComm messages
/// Follows the database table guidelines from the architecture document
/// </summary>
public class MessageRecord
{
    /// <summary>
    /// Primary key - surrogate ID
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// DIDComm message ID (from message.id)
    /// </summary>
    public string MessageId { get; set; } = string.Empty;

    /// <summary>
    /// Sender DID
    /// </summary>
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// Recipient DID
    /// </summary>
    public string To { get; set; } = string.Empty;

    /// <summary>
    /// Message type URI
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Message body (JSON serialized)
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// Attachments (JSON serialized array)
    /// </summary>
    public string? Attachments { get; set; }

    /// <summary>
    /// Thread ID for conversation threading
    /// </summary>
    public string? ThreadId { get; set; }

    /// <summary>
    /// Parent thread ID for nested conversations
    /// </summary>
    public string? ParentThreadId { get; set; }

    /// <summary>
    /// Unix timestamp when message was created
    /// </summary>
    public long CreatedTime { get; set; }

    /// <summary>
    /// Message expiration time
    /// </summary>
    public DateTime? ExpiresTime { get; set; }

    /// <summary>
    /// When message was sent
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// When message was received
    /// </summary>
    public DateTime? ReceivedAt { get; set; }

    /// <summary>
    /// Message status (sent, received, failed, read)
    /// </summary>
    public string Status { get; set; } = "pending";

    /// <summary>
    /// Message direction (in, out)
    /// </summary>
    public string? Direction { get; set; }

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    // Standard system fields following database guidelines
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
