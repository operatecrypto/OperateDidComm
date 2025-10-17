namespace OperateCrypto.DIDComm.SDK.Models;

/// <summary>
/// Configuration options for DIDComm SDK client
/// </summary>
public class DIDCommClientOptions
{
    /// <summary>
    /// API endpoint URL (e.g., "https://api.operatecrypto.com")
    /// </summary>
    public string ApiEndpoint { get; set; } = "http://localhost:5179";

    /// <summary>
    /// User's DID
    /// </summary>
    public string? Did { get; set; }

    /// <summary>
    /// JWT authentication token
    /// </summary>
    public string? AuthToken { get; set; }
}

/// <summary>
/// Options for sending a message
/// </summary>
public class MessageOptions
{
    /// <summary>
    /// Message type URI
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Thread ID for conversation threading
    /// </summary>
    public string? ThreadId { get; set; }

    /// <summary>
    /// Optional attachments
    /// </summary>
    public List<AttachmentDto>? Attachments { get; set; }
}

/// <summary>
/// Message filter for querying messages
/// </summary>
public class MessageFilter
{
    public string? Status { get; set; }
    public bool? Unread { get; set; }
    public string? From { get; set; }
    public string? ThreadId { get; set; }
    public DateTime? Since { get; set; }

    public string ToQueryString()
    {
        var parameters = new List<string>();

        if (!string.IsNullOrEmpty(Status))
            parameters.Add($"status={Status}");
        if (Unread.HasValue)
            parameters.Add($"unread={Unread.Value}");
        if (!string.IsNullOrEmpty(From))
            parameters.Add($"from={Uri.EscapeDataString(From)}");
        if (!string.IsNullOrEmpty(ThreadId))
            parameters.Add($"threadId={ThreadId}");
        if (Since.HasValue)
            parameters.Add($"since={Since.Value:O}");

        return parameters.Any() ? "?" + string.Join("&", parameters) : "";
    }
}
