using OperateCrypto.DIDComm.Data.Entities;
using System.Collections.Concurrent;

namespace OperateCrypto.DIDComm.Data.Repositories;

/// <summary>
/// In-memory implementation of message repository for testing without database
/// </summary>
public class InMemoryMessageRepository : IMessageRepository
{
    private readonly ConcurrentDictionary<Guid, MessageRecord> _messages = new();

    public Task<MessageRecord> AddAsync(MessageRecord message)
    {
        message.Id = Guid.NewGuid();
        message.CreatedOn = DateTime.UtcNow;
        message.LastModifiedOn = DateTime.UtcNow;
        _messages[message.Id] = message;
        return Task.FromResult(message);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_messages.TryRemove(id, out _));
    }

    public Task<MessageRecord?> GetByIdAsync(Guid id)
    {
        _messages.TryGetValue(id, out var message);
        return Task.FromResult(message);
    }

    public Task<MessageRecord?> GetByMessageIdAsync(string messageId)
    {
        var message = _messages.Values.FirstOrDefault(m => m.MessageId == messageId);
        return Task.FromResult(message);
    }

    public Task<List<MessageRecord>> GetMessagesByDidAsync(string did, int skip = 0, int take = 50)
    {
        var messages = _messages.Values
            .Where(m => m.From == did || m.To == did)
            .OrderByDescending(m => m.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToList();
        return Task.FromResult(messages);
    }

    public Task<List<MessageRecord>> GetMessagesByThreadAsync(string threadId)
    {
        var messages = _messages.Values
            .Where(m => m.ThreadId == threadId)
            .OrderBy(m => m.CreatedOn)
            .ToList();
        return Task.FromResult(messages);
    }

    public Task<List<MessageRecord>> GetReceivedMessagesAsync(string toDid, bool unreadOnly = false, int skip = 0, int take = 50)
    {
        var query = _messages.Values.Where(m => m.To == toDid);

        if (unreadOnly)
        {
            query = query.Where(m => m.Status != "read");
        }

        var messages = query
            .OrderByDescending(m => m.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToList();
        return Task.FromResult(messages);
    }

    public Task<List<MessageRecord>> GetSentMessagesAsync(string fromDid, int skip = 0, int take = 50)
    {
        var messages = _messages.Values
            .Where(m => m.From == fromDid)
            .OrderByDescending(m => m.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToList();
        return Task.FromResult(messages);
    }

    public Task<int> GetMessageCountAsync(string did, bool unreadOnly = false)
    {
        var query = _messages.Values.Where(m => m.To == did || m.From == did);

        if (unreadOnly)
        {
            query = query.Where(m => m.Status != "read");
        }

        return Task.FromResult(query.Count());
    }

    public Task<bool> MarkAsReadAsync(Guid id)
    {
        if (_messages.TryGetValue(id, out var message))
        {
            message.Status = "read";
            message.LastModifiedOn = DateTime.UtcNow;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<MessageRecord> UpdateAsync(MessageRecord message)
    {
        message.LastModifiedOn = DateTime.UtcNow;
        _messages[message.Id] = message;
        return Task.FromResult(message);
    }
}
