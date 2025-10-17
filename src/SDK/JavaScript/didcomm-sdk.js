/**
 * OperateCrypto DIDComm JavaScript SDK
 * Vanilla JavaScript SDK for DIDComm v2 messaging
 *
 * @version 1.0.0
 * @author OperateCrypto
 * @license MIT
 */

class DIDCommClient {
    /**
     * Creates a new DIDComm client
     * @param {Object} config - Configuration options
     * @param {string} config.apiEndpoint - API base URL
     * @param {string} config.did - User's DID
     * @param {string} config.token - JWT authentication token
     * @param {Function} config.onMessage - Callback for new messages
     * @param {number} config.pollingInterval - Polling interval in ms (default: 5000)
     */
    constructor(config) {
        this.apiEndpoint = config.apiEndpoint || 'http://localhost:5179';
        this.did = config.did;
        this.token = config.token;
        this.onMessage = config.onMessage || null;
        this.pollingInterval = config.pollingInterval || 5000;
        this.polling = false;
        this.pollIntervalId = null;
    }

    /**
     * Send a DIDComm message
     * @param {string} to - Recipient DID
     * @param {Object} body - Message body
     * @param {Object} options - Optional parameters
     * @returns {Promise<Object>} Send result
     */
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
            throw new Error(error.message || error.errorMessage || 'Failed to send message');
        }

        return await response.json();
    }

    /**
     * Get messages with filters
     * @param {Object} filter - Filter options
     * @returns {Promise<Array>} List of messages
     */
    async getMessages(filter = {}) {
        const params = new URLSearchParams();

        if (filter.status) params.append('status', filter.status);
        if (filter.unread !== undefined) params.append('unread', filter.unread);
        if (filter.from) params.append('from', filter.from);
        if (filter.threadId) params.append('threadId', filter.threadId);
        if (filter.since) params.append('since', filter.since.toISOString());
        if (!filter.did) params.append('did', this.did);

        const queryString = params.toString();
        const url = `${this.apiEndpoint}/api/didcomm/messages${queryString ? '?' + queryString : ''}`;

        const response = await fetch(url, {
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        });

        if (!response.ok) {
            throw new Error('Failed to fetch messages');
        }

        return await response.json();
    }

    /**
     * Start polling for new messages
     * @param {Function} callback - Callback function for new messages
     */
    startPolling(callback) {
        if (this.polling) {
            console.warn('Polling is already running');
            return;
        }

        this.polling = true;
        this.onMessage = callback || this.onMessage;

        this.pollIntervalId = setInterval(async () => {
            try {
                const messages = await this.getMessages({
                    status: 'received',
                    unread: true
                });

                if (messages.length > 0 && this.onMessage) {
                    this.onMessage(messages);
                }
            } catch (error) {
                console.error('Polling error:', error);
            }
        }, this.pollingInterval);

        console.log(`Started polling for messages every ${this.pollingInterval}ms`);
    }

    /**
     * Stop polling
     */
    stopPolling() {
        if (this.pollIntervalId) {
            clearInterval(this.pollIntervalId);
            this.pollIntervalId = null;
            this.polling = false;
            console.log('Stopped polling for messages');
        }
    }

    /**
     * Mark message as read
     * @param {string} messageId - Message GUID
     * @returns {Promise<boolean>} Success status
     */
    async markAsRead(messageId) {
        const response = await fetch(`${this.apiEndpoint}/api/didcomm/messages/${messageId}/read`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        });

        return response.ok;
    }

    /**
     * Send a trust ping to test connectivity
     * @param {string} to - Recipient DID
     * @returns {Promise<Object>} Ping result
     */
    async sendPing(to) {
        return await this.sendMessage(to, {
            comment: 'ping',
            timestamp: new Date().toISOString()
        }, {
            type: 'https://didcomm.org/trust-ping/2.0/ping'
        });
    }
}

// Export for module systems
if (typeof module !== 'undefined' && module.exports) {
    module.exports = DIDCommClient;
}

// Usage Example (commented out)
/*
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

// Stop polling when done
// client.stopPolling();
*/
