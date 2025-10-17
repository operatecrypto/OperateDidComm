using System.Text.Json.Serialization;

namespace OperateCrypto.DIDComm.Resolver.Models;

/// <summary>
/// Cryptographic proof for DID Document
/// </summary>
public class Proof
{
    /// <summary>
    /// Type of proof (e.g., "EcdsaSecp256k1Signature2019")
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Creation timestamp
    /// </summary>
    [JsonPropertyName("created")]
    public string? Created { get; set; }

    /// <summary>
    /// Verification method used for the proof
    /// </summary>
    [JsonPropertyName("verificationMethod")]
    public string? VerificationMethod { get; set; }

    /// <summary>
    /// Purpose of the proof
    /// </summary>
    [JsonPropertyName("proofPurpose")]
    public string? ProofPurpose { get; set; }

    /// <summary>
    /// JSON Web Signature (JWS)
    /// </summary>
    [JsonPropertyName("jws")]
    public string? Jws { get; set; }

    /// <summary>
    /// Proof value
    /// </summary>
    [JsonPropertyName("proofValue")]
    public string? ProofValue { get; set; }
}
