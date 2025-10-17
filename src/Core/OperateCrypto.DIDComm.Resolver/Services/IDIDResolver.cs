using OperateCrypto.DIDComm.Resolver.Models;

namespace OperateCrypto.DIDComm.Resolver.Services;

/// <summary>
/// DID Resolver service interface
/// </summary>
public interface IDIDResolver
{
    /// <summary>
    /// Resolves a DID to its DID Document
    /// </summary>
    /// <param name="did">The DID to resolve</param>
    /// <returns>The resolved DID Document</returns>
    Task<DIDDocument> ResolveDIDAsync(string did);

    /// <summary>
    /// Gets a service endpoint from a DID Document
    /// </summary>
    /// <param name="doc">The DID Document</param>
    /// <param name="serviceType">The service type to find</param>
    /// <returns>Service endpoint URL or null if not found</returns>
    string? GetServiceEndpoint(DIDDocument doc, string serviceType);

    /// <summary>
    /// Gets verification methods for a specific purpose
    /// </summary>
    /// <param name="doc">The DID Document</param>
    /// <param name="purpose">Purpose (authentication, keyAgreement, assertionMethod)</param>
    /// <returns>List of verification methods</returns>
    List<VerificationMethod> GetVerificationMethods(DIDDocument doc, string purpose);

    /// <summary>
    /// Gets the primary key agreement key (for encryption)
    /// </summary>
    /// <param name="did">The DID</param>
    /// <returns>Verification method for key agreement or null</returns>
    Task<VerificationMethod?> GetKeyAgreementKeyAsync(string did);

    /// <summary>
    /// Gets the primary authentication key (for signing)
    /// </summary>
    /// <param name="did">The DID</param>
    /// <returns>Verification method for authentication or null</returns>
    Task<VerificationMethod?> GetAuthenticationKeyAsync(string did);
}
