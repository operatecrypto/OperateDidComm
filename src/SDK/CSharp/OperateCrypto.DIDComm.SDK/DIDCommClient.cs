using System.Net.Http.Headers;
using System.Net.Http.Json;
using OperateCrypto.DIDComm.SDK.Models;

namespace OperateCrypto.DIDComm.SDK;

/// <summary>
/// C# SDK Client for OperateCrypto DIDComm API
/// Provides easy-to-use methods for sending and receiving DIDComm messages
/// </summary>
public class DIDCommClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly DIDCommClientOptions _options;
    private readonly string _did;
    private CancellationTokenSource? _pollingCancellation;
    private Task? _pollingTask;

    public DIDCommClient(DIDCommClientOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _did = options.Did ?? throw new ArgumentException("DID is required", nameof(options.Did));

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(options.ApiEndpoint)
        };

        if (!string.IsNullOrEmpty(options.AuthToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", options.AuthToken);
        }
    }

    /// <summary>
    /// Sends a DIDComm message
    /// </summary>
    public async Task<SendMessageResponse> SendMessageAsync(
        string to,
        object body,
        MessageOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var request = new SendMessageRequest
        {
            From = _did,
            To = to,
            Type = options?.Type ?? "https://didcomm.org/basicmessage/2.0/message",
            Body = body,
            ThreadId = options?.ThreadId,
            Attachments = options?.Attachments
        };

        var response = await _httpClient.PostAsJsonAsync("/api/didcomm/send", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<SendMessageResponse>(cancellationToken)
            ?? throw new InvalidOperationException("Failed to deserialize response");
    }

    /// <summary>
    /// Gets messages for the authenticated DID
    /// </summary>
    public async Task<List<MessageDto>> GetMessagesAsync(
        MessageFilter? filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = filter?.ToQueryString() ?? "?did=" + Uri.EscapeDataString(_did);
        var response = await _httpClient.GetAsync($"/api/didcomm/messages{query}", cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<MessageDto>>(cancellationToken)
            ?? new List<MessageDto>();
    }

    /// <summary>
    /// Marks a message as read
    /// </summary>
    public async Task<bool> MarkAsReadAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsync(
            $"/api/didcomm/messages/{messageId}/read", null, cancellationToken);

        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Starts polling for new messages
    /// </summary>
    public void StartPolling(Action<List<MessageDto>> onMessages, int intervalMs = 5000)
    {
        if (_pollingTask != null)
        {
            throw new InvalidOperationException("Polling is already running");
        }

        _pollingCancellation = new CancellationTokenSource();
        _pollingTask = Task.Run(async () =>
        {
            while (!_pollingCancellation.Token.IsCancellationRequested)
            {
                try
                {
                    var messages = await GetMessagesAsync(new MessageFilter
                    {
                        Status = "received",
                        Unread = true
                    }, _pollingCancellation.Token);

                    if (messages.Any())
                    {
                        onMessages?.Invoke(messages);
                    }
                }
                catch (Exception ex)
                {
                    // Log error (could add event handler for errors)
                    Console.WriteLine($"Polling error: {ex.Message}");
                }

                await Task.Delay(intervalMs, _pollingCancellation.Token);
            }
        }, _pollingCancellation.Token);
    }

    /// <summary>
    /// Stops polling for messages
    /// </summary>
    public void StopPolling()
    {
        _pollingCancellation?.Cancel();
        _pollingTask?.Wait(TimeSpan.FromSeconds(5));
        _pollingTask = null;
        _pollingCancellation = null;
    }

    public void Dispose()
    {
        StopPolling();
        _httpClient?.Dispose();
        _pollingCancellation?.Dispose();
    }
}
