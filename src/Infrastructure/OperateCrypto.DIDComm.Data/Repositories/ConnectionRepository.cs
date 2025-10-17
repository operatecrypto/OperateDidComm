using Microsoft.EntityFrameworkCore;
using OperateCrypto.DIDComm.Data.Context;
using OperateCrypto.DIDComm.Data.Entities;

namespace OperateCrypto.DIDComm.Data.Repositories;

/// <summary>
/// Repository implementation for managing DID connections
/// </summary>
public class ConnectionRepository : IConnectionRepository
{
    private readonly DIDCommDbContext _context;

    public ConnectionRepository(DIDCommDbContext context)
    {
        _context = context;
    }

    public async Task<Connection?> GetByIdAsync(Guid id)
    {
        return await _context.Connections
            .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
    }

    public async Task<List<Connection>> GetByDidAsync(string did)
    {
        return await _context.Connections
            .Where(c => (c.MyDid == did || c.TheirDid == did) && !c.Deleted)
            .OrderByDescending(c => c.CreatedOn)
            .ToListAsync();
    }

    public async Task<Connection?> GetConnectionAsync(string myDid, string theirDid)
    {
        return await _context.Connections
            .FirstOrDefaultAsync(c =>
                c.MyDid == myDid &&
                c.TheirDid == theirDid &&
                !c.Deleted);
    }

    public async Task<Connection> AddAsync(Connection connection)
    {
        connection.CreatedOn = DateTime.UtcNow;
        connection.LastModifiedOn = DateTime.UtcNow;

        _context.Connections.Add(connection);
        await _context.SaveChangesAsync();

        return connection;
    }

    public async Task UpdateAsync(Connection connection)
    {
        connection.LastModifiedOn = DateTime.UtcNow;

        _context.Connections.Update(connection);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var connection = await GetByIdAsync(id);
        if (connection != null)
        {
            connection.Deleted = true;
            connection.LastModifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Connection>> GetActiveConnectionsAsync(string did)
    {
        return await _context.Connections
            .Where(c =>
                (c.MyDid == did || c.TheirDid == did) &&
                c.State == "complete" &&
                !c.Deleted)
            .OrderByDescending(c => c.CreatedOn)
            .ToListAsync();
    }
}
