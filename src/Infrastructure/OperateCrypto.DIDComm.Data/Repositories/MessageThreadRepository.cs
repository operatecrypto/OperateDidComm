using Microsoft.EntityFrameworkCore;
using OperateCrypto.DIDComm.Data.Context;
using OperateCrypto.DIDComm.Data.Entities;

namespace OperateCrypto.DIDComm.Data.Repositories;

/// <summary>
/// Repository implementation for managing message threads
/// </summary>
public class MessageThreadRepository : IMessageThreadRepository
{
    private readonly DIDCommDbContext _context;

    public MessageThreadRepository(DIDCommDbContext context)
    {
        _context = context;
    }

    public async Task<MessageThread?> GetByIdAsync(Guid id)
    {
        return await _context.MessageThreads
            .FirstOrDefaultAsync(t => t.Id == id && !t.Deleted);
    }

    public async Task<MessageThread?> GetByThreadIdAsync(string threadId)
    {
        return await _context.MessageThreads
            .FirstOrDefaultAsync(t => t.ThreadId == threadId && !t.Deleted);
    }

    public async Task<List<MessageThread>> GetThreadsByDidAsync(string did)
    {
        return await _context.MessageThreads
            .Where(t =>
                t.Participants != null &&
                t.Participants.Contains(did) &&
                !t.Deleted)
            .OrderByDescending(t => t.UpdatedAt)
            .ToListAsync();
    }

    public async Task<List<MessageThread>> GetThreadsBetweenDidsAsync(string did1, string did2)
    {
        return await _context.MessageThreads
            .Where(t =>
                t.Participants != null &&
                t.Participants.Contains(did1) &&
                t.Participants.Contains(did2) &&
                !t.Deleted)
            .OrderByDescending(t => t.UpdatedAt)
            .ToListAsync();
    }

    public async Task<MessageThread> AddAsync(MessageThread thread)
    {
        thread.CreatedOn = DateTime.UtcNow;
        thread.LastModifiedOn = DateTime.UtcNow;
        thread.UpdatedAt = DateTime.UtcNow;

        _context.MessageThreads.Add(thread);
        await _context.SaveChangesAsync();

        return thread;
    }

    public async Task UpdateAsync(MessageThread thread)
    {
        thread.LastModifiedOn = DateTime.UtcNow;

        _context.MessageThreads.Update(thread);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var thread = await GetByIdAsync(id);
        if (thread != null)
        {
            thread.Deleted = true;
            thread.LastModifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<MessageThread>> GetActiveThreadsAsync(string did)
    {
        return await _context.MessageThreads
            .Where(t =>
                t.Participants != null &&
                t.Participants.Contains(did) &&
                !t.Deleted)
            .OrderByDescending(t => t.UpdatedAt)
            .ToListAsync();
    }
}
