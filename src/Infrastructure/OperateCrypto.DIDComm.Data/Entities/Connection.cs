namespace OperateCrypto.DIDComm.Data.Entities;

/// <summary>
/// Database entity for DID connections/relationships
/// </summary>
public class Connection
{
    /// <summary>
    /// Primary key - surrogate ID
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// My DID in this connection
    /// </summary>
    public string MyDid { get; set; } = string.Empty;

    /// <summary>
    /// Their DID in this connection
    /// </summary>
    public string TheirDid { get; set; } = string.Empty;

    /// <summary>
    /// Label/name for their identity
    /// </summary>
    public string? TheirLabel { get; set; }

    /// <summary>
    /// Label/name for my identity
    /// </summary>
    public string? MyLabel { get; set; }

    /// <summary>
    /// Connection state (invited, requested, responded, complete)
    /// </summary>
    public string State { get; set; } = "invited";

    /// <summary>
    /// Role in the connection (inviter, invitee)
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Additional connection data (JSON)
    /// </summary>
    public string? ConnectionData { get; set; }

    /// <summary>
    /// When connection was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When connection was last updated
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
