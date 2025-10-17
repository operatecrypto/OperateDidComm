# DIDComm Infrastructure Architecture & Design
## OperateCrypto Web3 Wallet Integration

### Project Overview
- **Company**: OperateDIDComm.com
- **Product**: Web3 Wallet with DID Comm capabilities
- **Integration**: AccumulateNetwork.io and Ethereum
- **DID Projects**: OperateID.com, OperateDID.com
- **DID Format**: `did:web:{identity}.operatedid.com`
- **Existing Resolver**: https://api.accumulateplatform.com/did/{identity}

### Technology Stack
- **Backend**: C# MVC Core (.NET 6/7)
- **Database**: SQL Server
- **Frontend**: Vanilla JavaScript, Bootstrap 5
- **Architecture Pattern**: Microservices with Single Responsibility Principle (SRP)
- **Communication**: Direct DIDComm via API endpoints (no mediator initially)

---

Use branding and color themes from OperateID.com

## System Architecture

### High-Level Components

```
┌─────────────────────────────────────────────────────────────┐
│                        Client Applications                   │
│  ┌────────────┐  ┌────────────┐  ┌────────────────────┐   │
│  │  Web App   │  │ Mobile App │  │  Partner Systems   │   │
│  │(Bootstrap) │  │            │  │   (External APIs)  │   │
│  └────────────┘  └────────────┘  └────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                          SDK Layer                           │
│  ┌─────────────────────┐        ┌────────────────────┐     │
│  │  JavaScript SDK     │        │     C# SDK         │     │
│  │  (Vanilla JS)       │        │  (NuGet Package)   │     │
│  └─────────────────────┘        └────────────────────┘     │
└─────────────────────────────────────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                    DIDComm API Layer                         │
│                    (C# MVC Core API)                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │
│  │   Send API   │  │  Receive API │  │  Message API  │     │
│  │  /api/send   │  │ /api/receive │  │ /api/messages │     │
│  └──────────────┘  └──────────────┘  └──────────────┘     │
└─────────────────────────────────────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                      Core Services                           │
│  ┌────────────────┐  ┌─────────────────┐  ┌──────────────┐ │
│  │ DIDComm Core   │  │  DID Resolver   │  │ Crypto Svc   │ │
│  │   Service      │  │    Service      │  │  (KMS)       │ │
│  └────────────────┘  └─────────────────┘  └──────────────┘ │
│  ┌────────────────┐  ┌─────────────────┐  ┌──────────────┐ │
│  │Message Handler │  │  Key Manager    │  │ Data Access  │ │
│  │   Service      │  │    Service      │  │   Layer      │ │
│  └────────────────┘  └─────────────────┘  └──────────────┘ │
└─────────────────────────────────────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                     Data Storage Layer                       │
│  ┌──────────────────────────────────────────────────────┐   │
│  │                    SQL Server                        │   │
│  │  ┌──────────┐  ┌──────────┐  ┌──────────────────┐  │   │
│  │  │ Messages │  │Connections│  │  DIDComm Keys    │  │   │
│  │  └──────────┘  └──────────┘  └──────────────────┘  │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                    External Integrations                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────┐  │
│  │OperateCrypto │  │  Accumulate  │  │    Ethereum      │  │
│  │   Wallet     │  │   Network    │  │    Network       │  │
│  └──────────────┘  └──────────────┘  └──────────────────┘  │
│  ┌──────────────────────────────────────────────────────┐  │
│  │         Existing DID Resolver API                    │  │
│  │    https://api.accumulateplatform.com/did/{id}      │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

---

## Component Specifications

### 1. DIDComm Core Library (`OperateCrypto.DIDComm.Core`)

**Purpose**: Core DIDComm v2 protocol implementation

**Responsibilities**:
- Message envelope creation and parsing
- JWE (JSON Web Encryption) handling
- JWS (JSON Web Signature) handling
- Message packing/unpacking
- DIDComm v2 compliance

**Key Classes**:

```csharp
namespace OperateCrypto.DIDComm.Core
{
    // Main message structure
    public class DIDCommMessage
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public long CreatedTime { get; set; }
        public DateTime ExpiresTime { get; set; }
        public object Body { get; set; }
        public List<Attachment> Attachments { get; set; }
        public string ThreadId { get; set; }
        public string ParentThreadId { get; set; }
    }

    // Encrypted envelope
    public class DIDCommEnvelope
    {
        public string Protected { get; set; }
        public string Iv { get; set; }
        public string Ciphertext { get; set; }
        public string Tag { get; set; }
        public List<Recipient> Recipients { get; set; }
    }

    // Core service interface
    public interface IDIDCommService
    {
        Task<DIDCommEnvelope> PackMessage(DIDCommMessage message, string toDid);
        Task<DIDCommMessage> UnpackMessage(DIDCommEnvelope envelope);
        Task<string> SignMessage(DIDCommMessage message);
        Task<bool> VerifyMessage(string signedMessage);
    }
}
```

---

### 2. DID Resolver Service (`OperateCrypto.DIDComm.Resolver`)

**Purpose**: Resolve DIDs to DID Documents using existing API

**Integration**: Uses `https://api.accumulateplatform.com/did/{identity}`

**Key Implementation**:

```csharp
namespace OperateCrypto.DIDComm.Resolver
{
    public class DIDDocument
    {
        public string Id { get; set; }
        public string Context { get; set; }
        public List<VerificationMethod> VerificationMethod { get; set; }
        public List<Service> Service { get; set; }
        public List<string> Authentication { get; set; }
        public List<string> KeyAgreement { get; set; }
        public List<string> AssertionMethod { get; set; }
    }

    public class VerificationMethod
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Controller { get; set; }
        public string PublicKeyJwk { get; set; }
        public string PublicKeyMultibase { get; set; }
    }

    public class Service
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string ServiceEndpoint { get; set; }
        public List<string> Accept { get; set; }
        public object RoutingKeys { get; set; }
    }

    public interface IDIDResolver
    {
        Task<DIDDocument> ResolveDID(string did);
        string GetServiceEndpoint(DIDDocument doc, string serviceType);
        List<VerificationMethod> GetVerificationMethods(DIDDocument doc, string purpose);
    }
}
```

---

### 3. Cryptographic Service (`OperateCrypto.DIDComm.Crypto`)

**Purpose**: Handle all cryptographic operations

**Key Features**:
- Integration with OperateCrypto wallet
- Key generation and management
- Encryption/Decryption operations
- Signing/Verification operations

```csharp
namespace OperateCrypto.DIDComm.Crypto
{
    public interface ICryptoService
    {
        Task<KeyPair> GenerateKeyPair(KeyType type);
        Task<byte[]> Encrypt(byte[] data, string recipientPublicKey);
        Task<byte[]> Decrypt(byte[] encryptedData, string privateKey);
        Task<string> Sign(byte[] data, string privateKey);
        Task<bool> Verify(byte[] data, string signature, string publicKey);
    }

    public class KeyPair
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public KeyType Type { get; set; }
        public string KeyId { get; set; }
    }

    public enum KeyType
    {
        Ed25519,
        X25519,
        Secp256k1,
        RSA
    }
}
```

---

### 4. API Controllers (`OperateCrypto.DIDComm.Api`)

**Purpose**: RESTful API endpoints for DIDComm communication

#### Send Message Endpoint

```csharp
[ApiController]
[Route("api/didcomm")]
public class DIDCommController : ControllerBase
{
    [HttpPost("send")]
    [Authorize]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        // 1. Validate request
        // 2. Create DIDComm message
        // 3. Resolve recipient DID
        // 4. Pack message (encrypt)
        // 5. Send to recipient endpoint
        // 6. Store message in database
        // 7. Return status
    }

    [HttpPost("receive")]
    public async Task<IActionResult> ReceiveMessage([FromBody] DIDCommEnvelope envelope)
    {
        // 1. Unpack message (decrypt)
        // 2. Verify signature
        // 3. Store in database
        // 4. Process based on message type
        // 5. Return acknowledgment or response
    }

    [HttpGet("messages")]
    [Authorize]
    public async Task<IActionResult> GetMessages([FromQuery] MessageFilter filter)
    {
        // 1. Authenticate user
        // 2. Query messages from database
        // 3. Apply filters
        // 4. Return paginated results
    }
}
```

---

### 5. Message Handler Service (`OperateCrypto.DIDComm.Handlers`)

**Purpose**: Process different DIDComm message types

```csharp
namespace OperateCrypto.DIDComm.Handlers
{
    public interface IMessageHandler
    {
        Task<DIDCommMessage> HandleMessage(DIDCommMessage message);
        bool CanHandle(string messageType);
    }

    public class BasicMessageHandler : IMessageHandler
    {
        public bool CanHandle(string messageType)
        {
            return messageType == "https://didcomm.org/basicmessage/2.0/message";
        }

        public async Task<DIDCommMessage> HandleMessage(DIDCommMessage message)
        {
            // Process basic message
            // Return response if needed
        }
    }

    public class TrustPingHandler : IMessageHandler
    {
        public bool CanHandle(string messageType)
        {
            return messageType == "https://didcomm.org/trust-ping/2.0/ping";
        }

        public async Task<DIDCommMessage> HandleMessage(DIDCommMessage message)
        {
            // Return ping response
        }
    }
}
```

---

### 6. Database Schema (SQL Server)

```sql
-- DIDComm Messages Table
CREATE TABLE [dbo].[Messages] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [MessageId] NVARCHAR(255) NOT NULL UNIQUE,
    [From] NVARCHAR(500) NOT NULL,
    [To] NVARCHAR(500) NOT NULL,
    [Type] NVARCHAR(500) NOT NULL,
    [Body] NVARCHAR(MAX), -- JSON content
    [Attachments] NVARCHAR(MAX), -- JSON array
    [ThreadId] NVARCHAR(255),
    [ParentThreadId] NVARCHAR(255),
    [CreatedTime] BIGINT,
    [ExpiresTime] DATETIME2,
    [SentAt] DATETIME2,
    [ReceivedAt] DATETIME2,
    [Status] NVARCHAR(50) NOT NULL, -- sent, received, failed, read
    [Direction] NVARCHAR(10), -- in, out
    [ErrorMessage] NVARCHAR(MAX),
    INDEX IX_Messages_FromTo ([From], [To]),
    INDEX IX_Messages_ThreadId (ThreadId),
    INDEX IX_Messages_Status (Status),
    INDEX IX_Messages_CreatedAt (ReceivedAt DESC, SentAt DESC)
);

-- Connections Table
CREATE TABLE [dbo].[Connections] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [MyDid] NVARCHAR(500) NOT NULL,
    [TheirDid] NVARCHAR(500) NOT NULL,
    [TheirLabel] NVARCHAR(255),
    [MyLabel] NVARCHAR(255),
    [State] NVARCHAR(50) NOT NULL, -- invited, requested, responded, complete
    [Role] NVARCHAR(50), -- inviter, invitee
    [ConnectionData] NVARCHAR(MAX), -- JSON with additional data
    [CreatedAt] DATETIME2 DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 DEFAULT GETUTCDATE(),
    UNIQUE INDEX IX_Connections_DIDs (MyDid, TheirDid)
);

-- DIDComm Keys Table
CREATE TABLE [dbo].[DIDCommKeys] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Did] NVARCHAR(500) NOT NULL,
    [KeyId] NVARCHAR(500) NOT NULL,
    [KeyType] NVARCHAR(50) NOT NULL, -- Ed25519, X25519, Secp256k1
    [PublicKey] NVARCHAR(MAX) NOT NULL,
    [PrivateKey] NVARCHAR(MAX), -- Encrypted with master key
    [Purpose] NVARCHAR(50), -- signing, keyAgreement, authentication
    [IsActive] BIT DEFAULT 1,
    [CreatedAt] DATETIME2 DEFAULT GETUTCDATE(),
    [RevokedAt] DATETIME2,
    INDEX IX_Keys_Did (Did),
    INDEX IX_Keys_Active (Did, IsActive)
);

-- Message Threads Table
CREATE TABLE [dbo].[MessageThreads] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [ThreadId] NVARCHAR(255) NOT NULL UNIQUE,
    [ParentThreadId] NVARCHAR(255),
    [Participants] NVARCHAR(MAX), -- JSON array of DIDs
    [Subject] NVARCHAR(500),
    [Context] NVARCHAR(MAX), -- JSON metadata
    [CreatedAt] DATETIME2 DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 DEFAULT GETUTCDATE(),
    INDEX IX_Threads_Parent (ParentThreadId)
);
```

---

### 7. JavaScript SDK (Vanilla JS)

```javascript
// didcomm-sdk.js
class DIDCommClient {
    constructor(config) {
        this.apiEndpoint = config.apiEndpoint || 'https://api.operatecrypto.com';
        this.did = config.did;
        this.token = config.token;
        this.onMessage = config.onMessage || null;
        this.pollingInterval = config.pollingInterval || 5000;
        this.polling = false;
    }

    // Send a DIDComm message
    async sendMessage(to, body, options = {}) {
        const message = {
            from: this.did,
            to: to,
            type: options.type || 'https://didcomm.org/basicmessage/2.0/message',
            body: body,
            threadId: options.threadId || null,
            attachments: options.attachments || []
        };

        const response = await fetch(`${this.apiEndpoint}/api/didcomm/send`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.token}`
            },
            body: JSON.stringify(message)
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || 'Failed to send message');
        }

        return await response.json();
    }

    // Get messages with filters
    async getMessages(filter = {}) {
        const params = new URLSearchParams(filter);
        const response = await fetch(`${this.apiEndpoint}/api/didcomm/messages?${params}`, {
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        });

        if (!response.ok) {
            throw new Error('Failed to fetch messages');
        }

        return await response.json();
    }

    // Start polling for new messages
    startPolling(callback) {
        if (this.polling) return;
        
        this.polling = true;
        this.pollInterval = setInterval(async () => {
            try {
                const messages = await this.getMessages({ 
                    status: 'received',
                    unread: true 
                });
                
                if (messages.length > 0 && callback) {
                    callback(messages);
                }
            } catch (error) {
                console.error('Polling error:', error);
            }
        }, this.pollingInterval);
    }

    // Stop polling
    stopPolling() {
        if (this.pollInterval) {
            clearInterval(this.pollInterval);
            this.polling = false;
        }
    }

    // Mark message as read
    async markAsRead(messageId) {
        const response = await fetch(`${this.apiEndpoint}/api/didcomm/messages/${messageId}/read`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        });

        return response.ok;
    }

    // Create a new connection
    async createConnection(theirDid, label) {
        const response = await fetch(`${this.apiEndpoint}/api/didcomm/connections`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.token}`
            },
            body: JSON.stringify({
                myDid: this.did,
                theirDid: theirDid,
                label: label
            })
        });

        return await response.json();
    }
}

// Usage Example
const client = new DIDCommClient({
    apiEndpoint: 'https://api.operatecrypto.com',
    did: 'did:web:alice.operatedid.com',
    token: 'eyJhbGciOiJIUzI1NiIs...'
});

// Send a message
try {
    const result = await client.sendMessage(
        'did:web:bob.operatedid.com',
        { 
            content: 'Hello Bob!',
            timestamp: new Date().toISOString()
        }
    );
    console.log('Message sent:', result.messageId);
} catch (error) {
    console.error('Failed to send:', error);
}

// Poll for messages
client.startPolling((messages) => {
    messages.forEach(async (msg) => {
        console.log('New message from:', msg.from);
        console.log('Content:', msg.body);
        
        // Mark as read
        await client.markAsRead(msg.id);
    });
});
```

---

### 8. C# SDK (NuGet Package)

```csharp
// OperateCrypto.DIDComm.SDK/DIDCommClient.cs
namespace OperateCrypto.DIDComm.SDK
{
    public class DIDCommClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _did;
        private readonly DIDCommClientOptions _options;

        public DIDCommClient(DIDCommClientOptions options)
        {
            _options = options;
            _did = options.Did;
            
            _httpClient = new HttpClient 
            { 
                BaseAddress = new Uri(options.ApiEndpoint) 
            };
            
            if (!string.IsNullOrEmpty(options.AuthToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", options.AuthToken);
            }
        }

        public async Task<SendMessageResponse> SendMessageAsync(
            string to, 
            object body, 
            MessageOptions options = null)
        {
            var request = new SendMessageRequest
            {
                From = _did,
                To = to,
                Type = options?.Type ?? "https://didcomm.org/basicmessage/2.0/message",
                Body = body,
                ThreadId = options?.ThreadId,
                Attachments = options?.Attachments
            };

            var response = await _httpClient.PostAsJsonAsync("/api/didcomm/send", request);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsAsync<SendMessageResponse>();
        }

        public async Task<List<DIDCommMessage>> GetMessagesAsync(MessageFilter filter = null)
        {
            var query = filter?.ToQueryString() ?? "";
            var response = await _httpClient.GetAsync($"/api/didcomm/messages{query}");
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsAsync<List<DIDCommMessage>>();
        }

        public async Task<bool> MarkAsReadAsync(string messageId)
        {
            var response = await _httpClient.PutAsync(
                $"/api/didcomm/messages/{messageId}/read", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<Connection> CreateConnectionAsync(string theirDid, string label)
        {
            var request = new CreateConnectionRequest
            {
                MyDid = _did,
                TheirDid = theirDid,
                Label = label
            };

            var response = await _httpClient.PostAsJsonAsync("/api/didcomm/connections", request);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsAsync<Connection>();
        }

        // Polling support
        public void StartPolling(Action<List<DIDCommMessage>> onMessages, int intervalMs = 5000)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        var messages = await GetMessagesAsync(new MessageFilter 
                        { 
                            Status = "received",
                            Unread = true 
                        });
                        
                        if (messages.Any())
                        {
                            onMessages?.Invoke(messages);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error
                    }
                    
                    await Task.Delay(intervalMs);
                }
            });
        }
    }

    // Supporting classes
    public class DIDCommClientOptions
    {
        public string ApiEndpoint { get; set; }
        public string Did { get; set; }
        public string AuthToken { get; set; }
    }

    public class MessageOptions
    {
        public string Type { get; set; }
        public string ThreadId { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    public class MessageFilter
    {
        public string Status { get; set; }
        public bool? Unread { get; set; }
        public string From { get; set; }
        public string ThreadId { get; set; }
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
}

// Usage Example
var client = new DIDCommClient(new DIDCommClientOptions
{
    ApiEndpoint = "https://api.operatecrypto.com",
    Did = "did:web:alice.operatedid.com",
    AuthToken = "eyJhbGciOiJIUzI1NiIs..."
});

// Send a message
var response = await client.SendMessageAsync(
    "did:web:bob.operatedid.com",
    new { content = "Hello Bob!", timestamp = DateTime.UtcNow }
);

Console.WriteLine($"Message sent: {response.MessageId}");

// Get messages
var messages = await client.GetMessagesAsync(new MessageFilter 
{ 
    Status = "received",
    Unread = true 
});

foreach (var msg in messages)
{
    Console.WriteLine($"From: {msg.From}");
    Console.WriteLine($"Content: {msg.Body}");
    
    await client.MarkAsReadAsync(msg.Id);
}
```

---

## Project Structure

```
OperateCrypto.DIDComm/
│
├── src/
│   ├── Core/
│   │   ├── OperateCrypto.DIDComm.Core/
│   │   │   ├── Models/
│   │   │   │   ├── DIDCommMessage.cs
│   │   │   │   ├── DIDCommEnvelope.cs
│   │   │   │   └── Attachment.cs
│   │   │   ├── Services/
│   │   │   │   ├── IDIDCommService.cs
│   │   │   │   └── DIDCommService.cs
│   │   │   └── OperateCrypto.DIDComm.Core.csproj
│   │   │
│   │   ├── OperateCrypto.DIDComm.Resolver/
│   │   │   ├── Models/
│   │   │   │   ├── DIDDocument.cs
│   │   │   │   └── VerificationMethod.cs
│   │   │   ├── Services/
│   │   │   │   ├── IDIDResolver.cs
│   │   │   │   └── AccumulateResolver.cs
│   │   │   └── OperateCrypto.DIDComm.Resolver.csproj
│   │   │
│   │   └── OperateCrypto.DIDComm.Crypto/
│   │       ├── Services/
│   │       │   ├── ICryptoService.cs
│   │       │   └── CryptoService.cs
│   │       ├── Models/
│   │       │   └── KeyPair.cs
│   │       └── OperateCrypto.DIDComm.Crypto.csproj
│   │
│   ├── Infrastructure/
│   │   ├── OperateCrypto.DIDComm.Data/
│   │   │   ├── Context/
│   │   │   │   └── DIDCommDbContext.cs
│   │   │   ├── Entities/
│   │   │   │   ├── MessageRecord.cs
│   │   │   │   ├── Connection.cs
│   │   │   │   └── DIDCommKey.cs
│   │   │   ├── Repositories/
│   │   │   │   ├── IMessageRepository.cs
│   │   │   │   └── MessageRepository.cs
│   │   │   └── OperateCrypto.DIDComm.Data.csproj
│   │   │
│   │   └── OperateCrypto.DIDComm.Handlers/
│   │       ├── IMessageHandler.cs
│   │       ├── BasicMessageHandler.cs
│   │       ├── TrustPingHandler.cs
│   │       └── OperateCrypto.DIDComm.Handlers.csproj
│   │
│   ├── API/
│   │   └── OperateCrypto.DIDComm.Api/
│   │       ├── Controllers/
│   │       │   ├── DIDCommController.cs
│   │       │   ├── ConnectionsController.cs
│   │       │   └── MessagesController.cs
│   │       ├── Models/
│   │       │   ├── SendMessageRequest.cs
│   │       │   └── SendMessageResponse.cs
│   │       ├── Middleware/
│   │       │   └── DIDAuthenticationMiddleware.cs
│   │       ├── Program.cs
│   │       ├── Startup.cs
│   │       ├── appsettings.json
│   │       └── OperateCrypto.DIDComm.Api.csproj
│   │
│   └── SDK/
│       ├── OperateCrypto.DIDComm.SDK/
│       │   ├── DIDCommClient.cs
│       │   ├── Models/
│       │   └── OperateCrypto.DIDComm.SDK.csproj
│       │
│       └── JavaScript/
│           ├── didcomm-sdk.js
│           ├── didcomm-sdk.min.js
│           └── examples/
│               ├── basic-messaging.html
│               └── connection-setup.html
│
├── database/
│   ├── Scripts/
│   │   ├── 001_InitialSchema.sql
│   │   ├── 002_AddIndexes.sql
│   │   └── 003_SeedData.sql
│   └── Migrations/
│       └── (EF Core migrations)
│
├── tests/
│   ├── Unit/
│   │   ├── OperateCrypto.DIDComm.Core.Tests/
│   │   └── OperateCrypto.DIDComm.Resolver.Tests/
│   ├── Integration/
│   │   └── OperateCrypto.DIDComm.Api.Tests/
│   └── E2E/
│       └── DIDCommFlow.Tests/
│
├── docs/
│   ├── API.md
│   ├── SDK-JavaScript.md
│   ├── SDK-CSharp.md
│   └── DIDComm-Protocols.md
│
├── samples/
│   ├── ConsoleApp/
│   └── WebApp/
│
├── .gitignore
├── README.md
├── LICENSE
└── OperateCrypto.DIDComm.sln
```

---

## Implementation Roadmap

### Phase 1: Foundation (Week 1-2)
- [ ] Set up project structure and solution
- [ ] Implement DIDComm Core library
- [ ] Integrate with existing DID Resolver
- [ ] Set up SQL Server database schema
- [ ] Implement basic cryptographic operations

### Phase 2: API Development (Week 3-4)
- [ ] Build MVC Core API controllers
- [ ] Implement message sending endpoint
- [ ] Implement message receiving endpoint
- [ ] Add authentication/authorization
- [ ] Create message storage and retrieval

### Phase 3: SDK Development (Week 5-6)
- [ ] Develop JavaScript SDK (Vanilla JS)
- [ ] Develop C# SDK (.NET Standard)
- [ ] Create usage examples
- [ ] Write SDK documentation

### Phase 4: Testing & Security (Week 7-8)
- [ ] Unit tests for all components
- [ ] Integration tests for API
- [ ] End-to-end testing
- [ ] Security audit
- [ ] Performance optimization

### Phase 5: Advanced Features (Week 9-10)
- [ ] Message threading support
- [ ] Connection protocol implementation
- [ ] Attachment handling
- [ ] Batch message processing
- [ ] Admin dashboard (Bootstrap 5)

### Phase 6: Production Preparation (Week 11-12)
- [ ] Deployment scripts
- [ ] Monitoring and logging
- [ ] Rate limiting
- [ ] API versioning
- [ ] Production documentation

---

## Security Considerations

### Encryption
- All DIDComm messages MUST be encrypted (JWE)
- Support for multiple encryption algorithms (X25519, P-256)
- Perfect forward secrecy where possible

### Key Management
- Private keys stored encrypted in SQL Server
- Master key rotation support
- Hardware Security Module (HSM) integration ready

### Authentication
- JWT tokens for API authentication
- DID-based authentication for message verification
- Rate limiting per DID

### Data Protection
- SQL Server Transparent Data Encryption (TDE)
- Column-level encryption for sensitive data
- Regular security audits

---

## Configuration Examples

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OperateCryptoDIDComm;Trusted_Connection=true;"
  },
  "DIDComm": {
    "ServiceEndpoint": "https://api.operatecrypto.com/api/didcomm/receive",
    "ResolverEndpoint": "https://api.accumulateplatform.com/did/",
    "MaxMessageSize": 10485760,
    "MessageRetentionDays": 30
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "OperateCrypto",
    "Audience": "DIDCommAPI",
    "ExpiryMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  }
}
```

### Startup.cs Configuration
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Database
        services.AddDbContext<DIDCommDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // DIDComm Services
        services.AddScoped<IDIDCommService, DIDCommService>();
        services.AddScoped<IDIDResolver, AccumulateResolver>();
        services.AddScoped<ICryptoService, CryptoService>();
        
        // Message Handlers
        services.AddScoped<IMessageHandler, BasicMessageHandler>();
        services.AddScoped<IMessageHandler, TrustPingHandler>();

        // Repositories
        services.AddScoped<IMessageRepository, MessageRepository>();

        // Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

        // API
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        // Caching
        services.AddMemoryCache();
        
        // HTTP Clients
        services.AddHttpClient();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

---

## Future Enhancements (Post-MVP)

### DIDComm Mediator Service
- Store-and-forward messaging
- Message routing
- Return routing
- WebSocket support for real-time delivery

### Additional Protocol Support
- Credential Exchange Protocol
- Present Proof Protocol
- Connection Protocol
- Discover Features Protocol

### Scaling Considerations
- Redis for caching and queuing
- RabbitMQ/Azure Service Bus for message queue
- Horizontal scaling with load balancing
- Database sharding for large-scale deployments

### Mobile SDKs
- React Native SDK
- Flutter SDK
- Native iOS SDK (Swift)
- Native Android SDK (Kotlin)

### Monitoring & Analytics
- Application Insights integration
- Custom metrics dashboard
- Message flow visualization
- Performance monitoring

---

## References

- [DIDComm v2 Specification](https://identity.foundation/didcomm-messaging/spec/)
- [W3C DID Core Specification](https://www.w3.org/TR/did-core/)
- [DID Web Method Specification](https://w3c-ccg.github.io/did-method-web/)
- [Accumulate Network Documentation](https://docs.accumulatenetwork.io/)
- [JSON Web Encryption (JWE)](https://tools.ietf.org/html/rfc7516)
- [JSON Web Signature (JWS)](https://tools.ietf.org/html/rfc7515)

---

## Support & Contact

- **Chief Architect**: [Your Name]
- **Company**: OperateCrypto.com
- **DID Projects**: OperateID.com, OperateDID.com
- **Documentation**: [Internal Wiki/Docs URL]
- **Support**: support@operatecrypto.com

---

*This document serves as the comprehensive architecture and design guide for the OperateCrypto DIDComm Infrastructure. It should be reviewed and updated regularly as the system evolves.*