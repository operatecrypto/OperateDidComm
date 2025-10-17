using System.Text.Json.Serialization;

namespace OperateCrypto.DIDComm.Resolver.Models;

/// <summary>
/// Represents a W3C DID Document
/// </summary>
public class DIDDocument
{
    /// <summary>
    /// JSON-LD context
    /// </summary>
    [JsonPropertyName("@context")]
    public List<string> Context { get; set; } = new();

    /// <summary>
    /// DID identifier
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// DID controller
    /// </summary>
    [JsonPropertyName("controller")]
    public string? Controller { get; set; }

    /// <summary>
    /// Verification methods (public keys)
    /// </summary>
    [JsonPropertyName("verificationMethod")]
    public List<VerificationMethod> VerificationMethod { get; set; } = new();

    /// <summary>
    /// Authentication verification methods
    /// </summary>
    [JsonPropertyName("authentication")]
    public List<object> Authentication { get; set; } = new();

    /// <summary>
    /// Assertion method verification methods
    /// </summary>
    [JsonPropertyName("assertionMethod")]
    public List<object> AssertionMethod { get; set; } = new();

    /// <summary>
    /// Key agreement verification methods (for encryption)
    /// </summary>
    [JsonPropertyName("keyAgreement")]
    public List<object>? KeyAgreement { get; set; }

    /// <summary>
    /// Capability invocation verification methods
    /// </summary>
    [JsonPropertyName("capabilityInvocation")]
    public List<object>? CapabilityInvocation { get; set; }

    /// <summary>
    /// Capability delegation verification methods
    /// </summary>
    [JsonPropertyName("capabilityDelegation")]
    public List<object>? CapabilityDelegation { get; set; }

    /// <summary>
    /// Services endpoints
    /// </summary>
    [JsonPropertyName("service")]
    public List<Service>? Service { get; set; }

    /// <summary>
    /// Created timestamp
    /// </summary>
    [JsonPropertyName("created")]
    public string? Created { get; set; }

    /// <summary>
    /// Updated timestamp
    /// </summary>
    [JsonPropertyName("updated")]
    public string? Updated { get; set; }

    /// <summary>
    /// Proof of document authenticity
    /// </summary>
    [JsonPropertyName("proof")]
    public Proof? Proof { get; set; }
}
