# OperateCrypto DIDComm JavaScript SDK

**Vanilla JavaScript SDK for DIDComm v2 Messaging** - Enables decentralized identity communication in web browsers and Node.js.

![JavaScript](https://img.shields.io/badge/JavaScript-ES6+-F7DF1E)
![DIDComm](https://img.shields.io/badge/DIDComm-v2-00A98F)

## Installation

### Browser (CDN)

```html
<script src="https://cdn.operatecrypto.com/didcomm-sdk.js"></script>
```

### Browser (Local)

```html
<script src="path/to/didcomm-sdk.js"></script>
```

### Node.js

```bash
npm install @operatecrypto/didcomm-sdk
```

```javascript
const DIDCommClient = require('@operatecrypto/didcomm-sdk');
```

## Quick Start

### Initialize the Client

```javascript
const client = new DIDCommClient({
    apiEndpoint: 'http://localhost:5179',
    did: 'did:web:alice.operatedid.com',
    token: 'your-jwt-token'
});
```

### Send a Message

```javascript
try {
    const result = await client.sendMessage(
        'did:web:bob.operatedid.com',
        {
            content: 'Hello Bob!',
            timestamp: new Date().toISOString()
        }
    );

    console.log('Message sent!', result.messageId);
} catch (error) {
    console.error('Failed to send:', error);
}
```

### Send a Trust Ping

```javascript
const result = await client.sendPing('did:web:bob.operatedid.com');
console.log('Ping sent successfully!');
```

### Retrieve Messages

```javascript
// Get all unread messages
const messages = await client.getMessages({
    status: 'received',
    unread: true
});

messages.forEach(msg => {
    console.log('From:', msg.from);
    console.log('Message:', msg.body);
});
```

### Real-time Message Polling

```javascript
// Start polling for new messages
client.startPolling((messages) => {
    messages.forEach(async (msg) => {
        console.log('New message from:', msg.from);
        console.log('Content:', JSON.parse(msg.body));

        // Mark as read
        await client.markAsRead(msg.id);
    });
});

// Stop polling when done
client.stopPolling();
```

## API Reference

### Constructor

```javascript
new DIDCommClient(config)
```

**Config Options:**
- `apiEndpoint` (string) - API base URL
- `did` (string) - Your DID identifier
- `token` (string) - JWT authentication token
- `onMessage` (function, optional) - Callback for new messages
- `pollingInterval` (number, optional) - Polling interval in ms (default: 5000)

### Methods

#### sendMessage(to, body, options)

Send a DIDComm message.

```javascript
await client.sendMessage(
    'did:web:bob.operatedid.com',
    { content: 'Hello!' },
    {
        type: 'https://didcomm.org/basicmessage/2.0/message',
        threadId: 'thread-123',
        attachments: []
    }
);
```

**Parameters:**
- `to` (string) - Recipient's DID
- `body` (object) - Message body
- `options` (object, optional):
  - `type` - Message type URI
  - `threadId` - Thread identifier for conversation threading
  - `attachments` - Array of attachment objects

**Returns:** Promise<{messageId, status, sentAt, success}>

#### getMessages(filter)

Retrieve messages with optional filtering.

```javascript
const messages = await client.getMessages({
    status: 'received',
    unread: true,
    from: 'did:web:bob.operatedid.com',
    threadId: 'thread-123',
    since: new Date('2024-01-01')
});
```

**Filter Options:**
- `status` - Message status (sent, received, failed)
- `unread` - Boolean to filter unread messages
- `from` - Filter by sender DID
- `threadId` - Filter by thread ID
- `since` - Date object for messages since timestamp

**Returns:** Promise<Array<MessageDto>>

#### startPolling(callback)

Start polling for new messages at the configured interval.

```javascript
client.startPolling((messages) => {
    console.log('Received', messages.length, 'new messages');
});
```

**Parameters:**
- `callback` (function) - Called with array of new messages

#### stopPolling()

Stop polling for messages.

```javascript
client.stopPolling();
```

#### markAsRead(messageId)

Mark a message as read.

```javascript
await client.markAsRead('message-guid');
```

**Parameters:**
- `messageId` (string) - Message GUID

**Returns:** Promise<boolean>

#### sendPing(to)

Send a trust ping to test connectivity.

```javascript
await client.sendPing('did:web:bob.operatedid.com');
```

**Parameters:**
- `to` (string) - Recipient's DID

**Returns:** Promise<{messageId, status, success}>

## Advanced Usage

### Message Threading

```javascript
// Start a conversation
const response1 = await client.sendMessage(
    'did:web:bob.operatedid.com',
    { content: 'Starting a conversation' }
);

// Reply in the same thread
await client.sendMessage(
    'did:web:bob.operatedid.com',
    { content: 'Follow-up message' },
    { threadId: response1.messageId }
);
```

### File Attachments

```javascript
// Convert file to base64
function fileToBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result.split(',')[1]);
        reader.onerror = reject;
        reader.readAsDataURL(file);
    });
}

// Send with attachment
const fileInput = document.getElementById('fileInput');
const file = fileInput.files[0];
const base64Data = await fileToBase64(file);

await client.sendMessage(
    'did:web:bob.operatedid.com',
    { content: 'Document attached' },
    {
        attachments: [{
            description: 'Contract PDF',
            filename: file.name,
            mediaType: file.type,
            base64Data: base64Data
        }]
    }
);
```

### Custom Polling Interval

```javascript
const client = new DIDCommClient({
    apiEndpoint: 'http://localhost:5179',
    did: 'did:web:alice.operatedid.com',
    token: 'your-jwt-token',
    pollingInterval: 10000 // Poll every 10 seconds
});
```

### Message Callback on Initialization

```javascript
const client = new DIDCommClient({
    apiEndpoint: 'http://localhost:5179',
    did: 'did:web:alice.operatedid.com',
    token: 'your-jwt-token',
    onMessage: (messages) => {
        console.log('New messages:', messages);
    }
});

// Start polling with the configured callback
client.startPolling();
```

## Complete Example

See `examples/basic-messaging.html` for a complete Bootstrap 5 application demonstrating:
- Client initialization
- Sending messages
- Sending trust pings
- Real-time message polling
- Marking messages as read
- Display UI with Bootstrap styling

## Error Handling

```javascript
try {
    const result = await client.sendMessage(
        'did:web:bob.operatedid.com',
        { content: 'Hello' }
    );
} catch (error) {
    if (error.message.includes('401')) {
        console.error('Authentication failed - check your token');
    } else if (error.message.includes('404')) {
        console.error('Recipient DID not found');
    } else {
        console.error('Error:', error.message);
    }
}
```

## Browser Compatibility

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

Requires support for:
- ES6+ (async/await, fetch API)
- Promise
- JSON

## Node.js Compatibility

- Node.js 16.x or higher
- Requires `node-fetch` for Node.js < 18

```bash
npm install node-fetch
```

## Security Notes

- Always use HTTPS endpoints in production
- Store JWT tokens securely (not in localStorage for sensitive apps)
- Validate message sources before processing
- Implement rate limiting for polling

## Examples

The `/examples` directory contains:
- `basic-messaging.html` - Complete messaging UI with Bootstrap 5
- `simple-send.html` - Minimal message sending example
- `polling-demo.html` - Real-time message polling demonstration

## Support

- **Documentation**: [https://docs.operatecrypto.com](https://docs.operatecrypto.com)
- **Issues**: [GitHub Issues](https://github.com/OperateCrypto/OperateDIDComm/issues)
- **Email**: support@operatecrypto.com

## License

MIT License - See LICENSE file for details

---

**Built by OperateCrypto** | [OperateID.com](https://operateid.com) | [DIDComm v2 Spec](https://identity.foundation/didcomm-messaging/spec/)
