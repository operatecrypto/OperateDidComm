using OperateCrypto.DIDComm.Data.Entities;

namespace OperateCrypto.DIDComm.Data.Repositories;

/// <summary>
/// Repository interface for DIDComm messages
/// </summary>
public interface IMessageRepository
{
    /// <summary>
    /// Gets a message by its ID
    /// </summary>
    Task<MessageRecord?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets a message by its DIDComm message ID
    /// </summary>
    Task<MessageRecord?> GetByMessageIdAsync(string messageId);

    /// <summary>
    /// Gets messages for a specific DID
    /// </summary>
    Task<List<MessageRecord>> GetMessagesByDidAsync(string did, int skip = 0, int take = 50);

    /// <summary>
    /// Gets messages in a conversation thread
    /// </summary>
    Task<List<MessageRecord>> GetMessagesByThreadAsync(string threadId);

    /// <summary>
    /// Gets received messages for a DID
    /// </summary>
    Task<List<MessageRecord>> GetReceivedMessagesAsync(string toDid, bool unreadOnly = false, int skip = 0, int take = 50);

    /// <summary>
    /// Gets sent messages from a DID
    /// </summary>
    Task<List<MessageRecord>> GetSentMessagesAsync(string fromDid, int skip = 0, int take = 50);

    /// <summary>
    /// Adds a new message
    /// </summary>
    Task<MessageRecord> AddAsync(MessageRecord message);

    /// <summary>
    /// Updates an existing message
    /// </summary>
    Task<MessageRecord> UpdateAsync(MessageRecord message);

    /// <summary>
    /// Marks a message as read
    /// </summary>
    Task<bool> MarkAsReadAsync(Guid id);

    /// <summary>
    /// Deletes a message (soft delete)
    /// </summary>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Gets message count for a DID
    /// </summary>
    Task<int> GetMessageCountAsync(string did, bool unreadOnly = false);
}
