using OperateCrypto.DIDComm.Data.Entities;

namespace OperateCrypto.DIDComm.Data.Repositories;

/// <summary>
/// Repository interface for managing DIDComm cryptographic keys
/// </summary>
public interface IDIDCommKeyRepository
{
    /// <summary>
    /// Get key by ID
    /// </summary>
    Task<DIDCommKey?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get all keys for a specific DID
    /// </summary>
    Task<List<DIDCommKey>> GetByDidAsync(string did);

    /// <summary>
    /// Get key by key ID (from DID Document)
    /// </summary>
    Task<DIDCommKey?> GetByKeyIdAsync(string keyId);

    /// <summary>
    /// Get keys by purpose (authentication, keyAgreement, etc.)
    /// </summary>
    Task<List<DIDCommKey>> GetByPurposeAsync(string did, string purpose);

    /// <summary>
    /// Add new key
    /// </summary>
    Task<DIDCommKey> AddAsync(DIDCommKey key);

    /// <summary>
    /// Update existing key
    /// </summary>
    Task UpdateAsync(DIDCommKey key);

    /// <summary>
    /// Delete key (soft delete)
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Get active keys for a DID
    /// </summary>
    Task<List<DIDCommKey>> GetActiveKeysAsync(string did);
}
