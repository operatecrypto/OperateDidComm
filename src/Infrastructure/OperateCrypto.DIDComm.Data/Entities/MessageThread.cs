namespace OperateCrypto.DIDComm.Data.Entities;

/// <summary>
/// Database entity for message threads/conversations
/// </summary>
public class MessageThread
{
    /// <summary>
    /// Primary key - surrogate ID
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Thread identifier (unique)
    /// </summary>
    public string ThreadId { get; set; } = string.Empty;

    /// <summary>
    /// Parent thread ID for nested threads
    /// </summary>
    public string? ParentThreadId { get; set; }

    /// <summary>
    /// Participants in the thread (JSON array of DIDs)
    /// </summary>
    public string? Participants { get; set; }

    /// <summary>
    /// Thread subject/topic
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Thread context/metadata (JSON)
    /// </summary>
    public string? Context { get; set; }

    /// <summary>
    /// When thread was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When thread was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

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
