using System.Text.Json;
using System.Text.Json.Serialization;

namespace OperateCrypto.DIDComm.Resolver.Models;

/// <summary>
/// Verification method (public key) in a DID Document
/// </summary>
public class VerificationMethod
{
    /// <summary>
    /// Verification method identifier
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Type of verification method (e.g., JsonWebKey2020, EcdsaSecp256k1VerificationKey2019)
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Controller of this verification method
    /// </summary>
    [JsonPropertyName("controller")]
    public string Controller { get; set; } = string.Empty;

    /// <summary>
    /// Public key in JWK format
    /// </summary>
    [JsonPropertyName("publicKeyJwk")]
    public JsonElement? PublicKeyJwk { get; set; }

    /// <summary>
    /// Public key in multibase format
    /// </summary>
    [JsonPropertyName("publicKeyMultibase")]
    public string? PublicKeyMultibase { get; set; }

    /// <summary>
    /// Public key in base58 format (legacy)
    /// </summary>
    [JsonPropertyName("publicKeyBase58")]
    public string? PublicKeyBase58 { get; set; }

    /// <summary>
    /// Blockchain account ID (for blockchain-based DIDs)
    /// </summary>
    [JsonPropertyName("blockchainAccountId")]
    public string? BlockchainAccountId { get; set; }
}
