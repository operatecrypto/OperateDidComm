using System.Text.Json;
using OperateCrypto.DIDComm.Resolver.Models;

namespace OperateCrypto.DIDComm.Resolver.Services;

/// <summary>
/// DID Resolver implementation using Accumulate Network API
/// Resolves did:web:{identity}.operatedid.com via https://api.accumulateplatform.com/did/{identity}
/// </summary>
public class AccumulateResolver : IDIDResolver
{
    private readonly HttpClient _httpClient;
    private readonly string _resolverEndpoint;
    private readonly JsonSerializerOptions _jsonOptions;

    public AccumulateResolver(HttpClient httpClient, string? resolverEndpoint = null)
    {
        _httpClient = httpClient;
        _resolverEndpoint = resolverEndpoint ?? "https://api.accumulateplatform.com/did";

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<DIDDocument> ResolveDIDAsync(string did)
    {
        if (string.IsNullOrWhiteSpace(did))
            throw new ArgumentException("DID cannot be null or empty", nameof(did));

        if (!did.StartsWith("did:"))
            throw new ArgumentException("Invalid DID format", nameof(did));

        // Extract identity from DID
        // Format: did:web:identity.operatedid.com -> identity
        var identity = ExtractIdentity(did);

        // Build API URL
        var apiUrl = $"{_resolverEndpoint}/{identity}";

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var didDocument = JsonSerializer.Deserialize<DIDDocument>(content, _jsonOptions);

            if (didDocument == null)
                throw new InvalidOperationException($"Failed to deserialize DID Document for {did}");

            return didDocument;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to resolve DID {did}: {ex.Message}", ex);
        }
    }

    public string? GetServiceEndpoint(DIDDocument doc, string serviceType)
    {
        if (doc.Service == null || !doc.Service.Any())
            return null;

        var service = doc.Service.FirstOrDefault(s =>
            s.Type.Equals(serviceType, StringComparison.OrdinalIgnoreCase));

        if (service == null)
            return null;

        // ServiceEndpoint can be a string or object
        if (service.ServiceEndpoint.ValueKind == JsonValueKind.String)
        {
            return service.ServiceEndpoint.GetString();
        }
        else if (service.ServiceEndpoint.ValueKind == JsonValueKind.Object)
        {
            // Try to get 'uri' property from object
            if (service.ServiceEndpoint.TryGetProperty("uri", out var uri))
            {
                return uri.GetString();
            }
        }

        return null;
    }

    public List<VerificationMethod> GetVerificationMethods(DIDDocument doc, string purpose)
    {
        var methods = new List<VerificationMethod>();

        List<object>? references = purpose.ToLower() switch
        {
            "authentication" => doc.Authentication,
            "assertionmethod" => doc.AssertionMethod,
            "keyagreement" => doc.KeyAgreement,
            "capabilityinvocation" => doc.CapabilityInvocation,
            "capabilitydelegation" => doc.CapabilityDelegation,
            _ => null
        };

        if (references == null)
            return methods;

        foreach (var reference in references)
        {
            // Reference can be a string (ID) or embedded verification method object
            if (reference is string id)
            {
                var method = doc.VerificationMethod.FirstOrDefault(vm => vm.Id == id);
                if (method != null)
                    methods.Add(method);
            }
            else if (reference is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.String)
                {
                    var elementId = element.GetString();
                    var method = doc.VerificationMethod.FirstOrDefault(vm => vm.Id == elementId);
                    if (method != null)
                        methods.Add(method);
                }
                else if (element.ValueKind == JsonValueKind.Object)
                {
                    var method = JsonSerializer.Deserialize<VerificationMethod>(element.GetRawText(), _jsonOptions);
                    if (method != null)
                        methods.Add(method);
                }
            }
        }

        return methods;
    }

    public async Task<VerificationMethod?> GetKeyAgreementKeyAsync(string did)
    {
        var doc = await ResolveDIDAsync(did);
        var methods = GetVerificationMethods(doc, "keyAgreement");
        return methods.FirstOrDefault();
    }

    public async Task<VerificationMethod?> GetAuthenticationKeyAsync(string did)
    {
        var doc = await ResolveDIDAsync(did);
        var methods = GetVerificationMethods(doc, "authentication");
        return methods.FirstOrDefault();
    }

    private string ExtractIdentity(string did)
    {
        // did:web:identity.operatedid.com -> identity
        var parts = did.Split(':');
        if (parts.Length < 3)
            throw new ArgumentException("Invalid DID format", nameof(did));

        var domain = parts[2];
        var domainParts = domain.Split('.');
        return domainParts[0]; // Get the first part (identity)
    }
}
