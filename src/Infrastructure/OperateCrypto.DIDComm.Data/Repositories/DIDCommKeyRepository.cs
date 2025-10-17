using Microsoft.EntityFrameworkCore;
using OperateCrypto.DIDComm.Data.Context;
using OperateCrypto.DIDComm.Data.Entities;

namespace OperateCrypto.DIDComm.Data.Repositories;

/// <summary>
/// Repository implementation for managing DIDComm cryptographic keys
/// </summary>
public class DIDCommKeyRepository : IDIDCommKeyRepository
{
    private readonly DIDCommDbContext _context;

    public DIDCommKeyRepository(DIDCommDbContext context)
    {
        _context = context;
    }

    public async Task<DIDCommKey?> GetByIdAsync(Guid id)
    {
        return await _context.DIDCommKeys
            .FirstOrDefaultAsync(k => k.Id == id && !k.Deleted);
    }

    public async Task<List<DIDCommKey>> GetByDidAsync(string did)
    {
        return await _context.DIDCommKeys
            .Where(k => k.Did == did && !k.Deleted)
            .OrderByDescending(k => k.CreatedOn)
            .ToListAsync();
    }

    public async Task<DIDCommKey?> GetByKeyIdAsync(string keyId)
    {
        return await _context.DIDCommKeys
            .FirstOrDefaultAsync(k => k.KeyId == keyId && !k.Deleted);
    }

    public async Task<List<DIDCommKey>> GetByPurposeAsync(string did, string purpose)
    {
        return await _context.DIDCommKeys
            .Where(k =>
                k.Did == did &&
                k.Purpose == purpose &&
                !k.Deleted)
            .OrderByDescending(k => k.CreatedOn)
            .ToListAsync();
    }

    public async Task<DIDCommKey> AddAsync(DIDCommKey key)
    {
        key.CreatedOn = DateTime.UtcNow;
        key.LastModifiedOn = DateTime.UtcNow;

        _context.DIDCommKeys.Add(key);
        await _context.SaveChangesAsync();

        return key;
    }

    public async Task UpdateAsync(DIDCommKey key)
    {
        key.LastModifiedOn = DateTime.UtcNow;

        _context.DIDCommKeys.Update(key);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var key = await GetByIdAsync(id);
        if (key != null)
        {
            key.Deleted = true;
            key.LastModifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<DIDCommKey>> GetActiveKeysAsync(string did)
    {
        return await _context.DIDCommKeys
            .Where(k =>
                k.Did == did &&
                !k.Deleted)
            .OrderByDescending(k => k.CreatedOn)
            .ToListAsync();
    }
}
