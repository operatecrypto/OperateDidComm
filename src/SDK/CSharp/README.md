# OperateCrypto DIDComm C# SDK

**.NET SDK for DIDComm v2 Messaging** - Enables decentralized identity communication in C# applications.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![NuGet](https://img.shields.io/badge/NuGet-1.0.0-blue)

## Installation

```bash
dotnet add package OperateCrypto.DIDComm.SDK
```

Or via NuGet Package Manager:

```
Install-Package OperateCrypto.DIDComm.SDK
```

## Quick Start

### Initialize the Client

```csharp
using OperateCrypto.DIDComm.SDK;
using OperateCrypto.DIDComm.SDK.Models;

var client = new DIDCommClient(new DIDCommClientOptions
{
    ApiEndpoint = "http://localhost:5179",
    Did = "did:web:alice.operatedid.com",
    AuthToken = "your-jwt-token"
});
```

### Send a Message

```csharp
var response = await client.SendMessageAsync(
    to: "did:web:bob.operatedid.com",
    body: new
    {
        content = "Hello Bob!",
        timestamp = DateTime.UtcNow
    }
);

Console.WriteLine($"Message sent! ID: {response.MessageId}");
```

### Send a Trust Ping

```csharp
var pingResponse = await client.SendPingAsync("did:web:bob.operatedid.com");
Console.WriteLine($"Ping sent successfully: {pingResponse.Success}");
```

### Retrieve Messages

```csharp
// Get all unread messages
var messages = await client.GetMessagesAsync(new MessageFilter
{
    Unread = true,
    Status = "received"
});

foreach (var message in messages)
{
    Console.WriteLine($"From: {message.From}");
    Console.WriteLine($"Body: {message.Body}");

    // Mark as read
    await client.MarkAsReadAsync(message.Id);
}
```

### Advanced: Message Threading

```csharp
// Send a message in a thread
var response = await client.SendMessageAsync(
    to: "did:web:bob.operatedid.com",
    body: new { content = "Reply to your message" },
    options: new MessageOptions
    {
        ThreadId = "thread-123",
        Type = "https://didcomm.org/basicmessage/2.0/message"
    }
);
```

### Advanced: Attachments

```csharp
var response = await client.SendMessageAsync(
    to: "did:web:bob.operatedid.com",
    body: new { content = "Document attached" },
    options: new MessageOptions
    {
        Attachments = new List<AttachmentDto>
        {
            new AttachmentDto
            {
                Description = "Contract PDF",
                Filename = "contract.pdf",
                MediaType = "application/pdf",
                Base64Data = Convert.ToBase64String(pdfBytes)
            }
        }
    }
);
```

## API Reference

### DIDCommClient Constructor

```csharp
public DIDCommClient(DIDCommClientOptions options)
```

**Options:**
- `ApiEndpoint` (string) - API base URL
- `Did` (string) - Your DID identifier
- `AuthToken` (string) - JWT authentication token

### Methods

#### SendMessageAsync

```csharp
Task<SendMessageResponse> SendMessageAsync(
    string to,
    object body,
    MessageOptions? options = null
)
```

Send a DIDComm message to another DID.

**Parameters:**
- `to` - Recipient's DID
- `body` - Message body (any serializable object)
- `options` - Optional parameters (type, threadId, attachments)

**Returns:** `SendMessageResponse` with messageId and status

#### GetMessagesAsync

```csharp
Task<List<MessageDto>> GetMessagesAsync(MessageFilter? filter = null)
```

Retrieve messages with optional filtering.

**Filter Options:**
- `Status` - Message status (sent, received, failed)
- `Unread` - Filter unread messages only
- `From` - Filter by sender DID
- `ThreadId` - Filter by thread
- `Since` - Messages since timestamp

#### MarkAsReadAsync

```csharp
Task<bool> MarkAsReadAsync(Guid messageId)
```

Mark a message as read.

#### SendPingAsync

```csharp
Task<SendMessageResponse> SendPingAsync(string to)
```

Send a trust ping to test connectivity.

## Configuration

### appsettings.json Example

```json
{
  "DIDComm": {
    "ApiEndpoint": "https://api.operatecrypto.com",
    "Did": "did:web:myidentity.operatedid.com",
    "AuthToken": "eyJhbGciOiJIUzI1NiIs..."
  }
}
```

### Dependency Injection Setup

```csharp
// In Program.cs or Startup.cs
services.AddHttpClient<DIDCommClient>();

services.AddScoped<DIDCommClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var httpClient = sp.GetRequiredService<HttpClient>();

    return new DIDCommClient(new DIDCommClientOptions
    {
        ApiEndpoint = config["DIDComm:ApiEndpoint"],
        Did = config["DIDComm:Did"],
        AuthToken = config["DIDComm:AuthToken"]
    });
});
```

## Error Handling

```csharp
try
{
    var response = await client.SendMessageAsync(
        "did:web:bob.operatedid.com",
        new { content = "Hello" }
    );
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Network error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Examples

See the `/examples` folder for complete sample applications:
- Console application with message sending/receiving
- ASP.NET Core integration
- Background message polling service

## Requirements

- .NET 8.0 or higher
- Valid DID identifier
- JWT authentication token from OperateCrypto API

## Support

- **Documentation**: [https://docs.operatecrypto.com](https://docs.operatecrypto.com)
- **Issues**: [GitHub Issues](https://github.com/OperateCrypto/OperateDIDComm/issues)
- **Email**: support@operatecrypto.com

## License

MIT License - See LICENSE file for details

---

**Built by OperateCrypto** | [OperateID.com](https://operateid.com)
