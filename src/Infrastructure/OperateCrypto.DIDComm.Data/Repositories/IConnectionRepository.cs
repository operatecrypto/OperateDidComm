using OperateCrypto.DIDComm.Data.Entities;

namespace OperateCrypto.DIDComm.Data.Repositories;

/// <summary>
/// Repository interface for managing DID connections
/// </summary>
public interface IConnectionRepository
{
    /// <summary>
    /// Get connection by ID
    /// </summary>
    Task<Connection?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get all connections for a specific DID
    /// </summary>
    Task<List<Connection>> GetByDidAsync(string did);

    /// <summary>
    /// Get connection between two DIDs
    /// </summary>
    Task<Connection?> GetConnectionAsync(string myDid, string theirDid);

    /// <summary>
    /// Add new connection
    /// </summary>
    Task<Connection> AddAsync(Connection connection);

    /// <summary>
    /// Update existing connection
    /// </summary>
    Task UpdateAsync(Connection connection);

    /// <summary>
    /// Delete connection (soft delete)
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Get all active connections for a DID
    /// </summary>
    Task<List<Connection>> GetActiveConnectionsAsync(string did);
}
