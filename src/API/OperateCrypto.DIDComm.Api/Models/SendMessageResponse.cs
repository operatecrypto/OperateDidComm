namespace OperateCrypto.DIDComm.Api.Models;

/// <summary>
/// Response model for send message operation
/// </summary>
public class SendMessageResponse
{
    /// <summary>
    /// Unique message ID
    /// </summary>
    public string MessageId { get; set; } = string.Empty;

    /// <summary>
    /// Message status
    /// </summary>
    public string Status { get; set; } = "sent";

    /// <summary>
    /// Timestamp when message was sent
    /// </summary>
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Success indicator
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}
