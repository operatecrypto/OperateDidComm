using Microsoft.EntityFrameworkCore;
using OperateCrypto.DIDComm.Data.Context;
using OperateCrypto.DIDComm.Data.Entities;

namespace OperateCrypto.DIDComm.Data.Repositories;

/// <summary>
/// Repository implementation for DIDComm messages
/// </summary>
public class MessageRepository : IMessageRepository
{
    private readonly DIDCommDbContext _context;

    public MessageRepository(DIDCommDbContext context)
    {
        _context = context;
    }

    public async Task<MessageRecord?> GetByIdAsync(Guid id)
    {
        return await _context.Messages
            .Where(m => !m.Deleted)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<MessageRecord?> GetByMessageIdAsync(string messageId)
    {
        return await _context.Messages
            .Where(m => !m.Deleted)
            .FirstOrDefaultAsync(m => m.MessageId == messageId);
    }

    public async Task<List<MessageRecord>> GetMessagesByDidAsync(string did, int skip = 0, int take = 50)
    {
        return await _context.Messages
            .Where(m => !m.Deleted && (m.From == did || m.To == did))
            .OrderByDescending(m => m.ReceivedAt ?? m.SentAt ?? m.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<List<MessageRecord>> GetMessagesByThreadAsync(string threadId)
    {
        return await _context.Messages
            .Where(m => !m.Deleted && m.ThreadId == threadId)
            .OrderBy(m => m.CreatedTime)
            .ToListAsync();
    }

    public async Task<List<MessageRecord>> GetReceivedMessagesAsync(string toDid, bool unreadOnly = false, int skip = 0, int take = 50)
    {
        var query = _context.Messages
            .Where(m => !m.Deleted && m.To == toDid && m.Direction == "in");

        if (unreadOnly)
        {
            query = query.Where(m => m.Status != "read");
        }

        return await query
            .OrderByDescending(m => m.ReceivedAt ?? m.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<List<MessageRecord>> GetSentMessagesAsync(string fromDid, int skip = 0, int take = 50)
    {
        return await _context.Messages
            .Where(m => !m.Deleted && m.From == fromDid && m.Direction == "out")
            .OrderByDescending(m => m.SentAt ?? m.CreatedOn)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<MessageRecord> AddAsync(MessageRecord message)
    {
        message.CreatedOn = DateTime.UtcNow;
        message.LastModifiedOn = DateTime.UtcNow;

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<MessageRecord> UpdateAsync(MessageRecord message)
    {
        message.LastModifiedOn = DateTime.UtcNow;

        _context.Messages.Update(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<bool> MarkAsReadAsync(Guid id)
    {
        var message = await GetByIdAsync(id);
        if (message == null)
            return false;

        message.Status = "read";
        message.LastModifiedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var message = await GetByIdAsync(id);
        if (message == null)
            return false;

        message.Deleted = true;
        message.LastModifiedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetMessageCountAsync(string did, bool unreadOnly = false)
    {
        var query = _context.Messages
            .Where(m => !m.Deleted && m.To == did);

        if (unreadOnly)
        {
            query = query.Where(m => m.Status != "read");
        }

        return await query.CountAsync();
    }
}
