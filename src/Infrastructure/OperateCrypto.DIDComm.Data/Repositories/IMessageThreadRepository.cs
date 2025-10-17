using OperateCrypto.DIDComm.Data.Entities;

namespace OperateCrypto.DIDComm.Data.Repositories;

/// <summary>
/// Repository interface for managing message threads
/// </summary>
public interface IMessageThreadRepository
{
    /// <summary>
    /// Get thread by ID
    /// </summary>
    Task<MessageThread?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get thread by thread ID
    /// </summary>
    Task<MessageThread?> GetByThreadIdAsync(string threadId);

    /// <summary>
    /// Get all threads for a specific DID
    /// </summary>
    Task<List<MessageThread>> GetThreadsByDidAsync(string did);

    /// <summary>
    /// Get threads between two DIDs
    /// </summary>
    Task<List<MessageThread>> GetThreadsBetweenDidsAsync(string did1, string did2);

    /// <summary>
    /// Add new thread
    /// </summary>
    Task<MessageThread> AddAsync(MessageThread thread);

    /// <summary>
    /// Update existing thread
    /// </summary>
    Task UpdateAsync(MessageThread thread);

    /// <summary>
    /// Delete thread (soft delete)
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Get active threads for a DID
    /// </summary>
    Task<List<MessageThread>> GetActiveThreadsAsync(string did);
}
