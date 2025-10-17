using System.Text.Json;
using System.Text.Json.Serialization;

namespace OperateCrypto.DIDComm.Resolver.Models;

/// <summary>
/// Service endpoint in a DID Document
/// </summary>
public class Service
{
    /// <summary>
    /// Service identifier
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Service type (e.g., "DIDCommMessaging", "OperateID")
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Service endpoint URL or object
    /// </summary>
    [JsonPropertyName("serviceEndpoint")]
    public JsonElement ServiceEndpoint { get; set; }

    /// <summary>
    /// Description of the service
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Accepted message types (for DIDComm services)
    /// </summary>
    [JsonPropertyName("accept")]
    public List<string>? Accept { get; set; }

    /// <summary>
    /// Routing keys for message forwarding
    /// </summary>
    [JsonPropertyName("routingKeys")]
    public List<string>? RoutingKeys { get; set; }

    /// <summary>
    /// Custom OID data for OperateID service
    /// </summary>
    [JsonPropertyName("oid")]
    public JsonElement? Oid { get; set; }
}
