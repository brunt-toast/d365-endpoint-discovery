using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Responses;
using Newtonsoft.Json;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;

internal class ApplicationAxAuthService : IAxAuthService
{
    private readonly IAxConfig _config;
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    private TokenResponse? _cachedResponse;
    private readonly SemaphoreSlim _tokenLock = new(1, 1);

    public ApplicationAxAuthService(IAxConfig config, ILogger logger, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetBearerToken()
    {
        if (TryGetCachedToken(out var token))
        {
            return token;
        }

        await _tokenLock.WaitAsync();
        try
        {
            if (TryGetCachedToken(out token))
            {
                return token;
            }

            _cachedResponse = await RequestBearerToken();
            return _cachedResponse?.AccessToken ?? string.Empty;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private bool TryGetCachedToken(out string token)
    {
        token = string.Empty;

        if (_cachedResponse is null || DateTimeOffset.FromUnixTimeSeconds(_cachedResponse.ExpiresOn) < DateTimeOffset.Now)
        {
            return false;
        }

        token = _cachedResponse.AccessToken;
        return true;
    }

    private async Task<TokenResponse?> RequestBearerToken()
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _config.TokenRequestEndpoint)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _config.ClientId,
                ["client_secret"] = _config.ClientSecret,
                ["grant_type"] = "client_credentials",
                ["resource"] = _config.Resource
            })
        };

        var client = _httpClientFactory.CreateClient();
        HttpResponseMessage response = await client.SendAsync(request);

        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("A request for a bearer token returned HTTP status {statusInt} ({status}). " +
                             "Expect cascading failures." +
                             "Content was: {newLine}{content}", (int)response.StatusCode, response.StatusCode, Environment.NewLine, content);
            return null;
        }

        return JsonConvert.DeserializeObject<TokenResponse>(content);
    }
}
