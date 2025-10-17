namespace OperateCrypto.DIDComm.SDK.Models;

/// <summary>
/// Send message request
/// </summary>
public class SendMessageRequest
{
    public string? From { get; set; }
    public string To { get; set; } = string.Empty;
    public string? Type { get; set; }
    public object Body { get; set; } = new();
    public string? ThreadId { get; set; }
    public List<AttachmentDto>? Attachments { get; set; }
}

/// <summary>
/// Send message response
/// </summary>
public class SendMessageResponse
{
    public string MessageId { get; set; } = string.Empty;
    public string Status { get; set; } = "sent";
    public DateTime SentAt { get; set; }
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Message DTO
/// </summary>
public class MessageDto
{
    public Guid Id { get; set; }
    public string MessageId { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Body { get; set; }
    public string? ThreadId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Direction { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? ReceivedAt { get; set; }
}

/// <summary>
/// Attachment DTO
/// </summary>
public class AttachmentDto
{
    public string? Description { get; set; }
    public string? Filename { get; set; }
    public string? MediaType { get; set; }
    public string? Base64Data { get; set; }
}
