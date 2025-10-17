# OperateCrypto DIDComm Infrastructure

**DIDComm v2 Messaging Infrastructure** for OperateCrypto Web3 Wallet Integration with AccumulateNetwork.io and Ethereum.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![DIDComm](https://img.shields.io/badge/DIDComm-v2-00A98F)
![License](https://img.shields.io/badge/license-MIT-blue)

## Overview

This project provides a complete DIDComm v2 implementation for decentralized identity communication, specifically designed for the OperateCrypto Web3 wallet ecosystem.

### Key Features

- ✅ Full DIDComm v2 Protocol Implementation
- ✅ Integration with Accumulate Network DID Resolver
- ✅ SQL Server Database with EF Core
- ✅ RESTful API with JWT Authentication
- ✅ C# SDK (NuGet Package)
- ✅ JavaScript SDK (Vanilla JS)
- ✅ Message Threading & Attachments
- ✅ Trust Ping Protocol
- ✅ Swagger/OpenAPI Documentation

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server 2019+
- Visual Studio 2022 or VS Code

### Quick Start

1. **Clone and restore**
   ```bash
   git clone https://github.com/OperateCrypto/OperateDIDComm.git
   cd OperateDIDComm
   dotnet restore
   ```

2. **Set up database** - Run SQL scripts in `database/Scripts/`

3. **Update** `appsettings.json` with your connection string

4. **Run the API**
   ```bash
   cd src/API/OperateCrypto.DIDComm.Api
   dotnet run
   ```

5. **Access Swagger UI** at `http://localhost:5179`

## Usage

### C# SDK

```csharp
var client = new DIDCommClient(new DIDCommClientOptions
{
    ApiEndpoint = "https://api.operatecrypto.com",
    Did = "did:web:alice.operatedid.com",
    AuthToken = "your-jwt-token"
});

await client.SendMessageAsync("did:web:bob.operatedid.com",
    new { content = "Hello!" });
```

### JavaScript SDK

```javascript
const client = new DIDCommClient({
    apiEndpoint: 'https://api.operatecrypto.com',
    did: 'did:web:alice.operatedid.com',
    token: 'your-jwt-token'
});

await client.sendMessage('did:web:bob.operatedid.com', {
    content: 'Hello Bob!'
});
```

## Project Structure

See `instruction/didcomm-architecture-design.md` for complete architecture details.

## License

MIT License

## Support

- **Documentation**: See `/instruction` folder
- **Contact**: support@operatecrypto.com

---

**Built by OperateCrypto Team** | [OperateID.com](https://operateid.com) | [DIDComm v2 Spec](https://identity.foundation/didcomm-messaging/spec/)
