using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

internal class AxAuthService
{
    private readonly IAxConfig _config;
    private readonly ILogger _logger;

    private TokenResponse? _cachedResponse;

    public AxAuthService(IAxConfig config, ILogger logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<string> GetBearerToken()
    {
        if (_cachedResponse is not null && DateTimeOffset.FromUnixTimeSeconds(_cachedResponse.ExpiresOn) >= DateTimeOffset.Now)
        {
            return _cachedResponse.AccessToken;
        }

        using HttpClient client = new();

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
        HttpResponseMessage response = await client.SendAsync(request);

        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("A request for a bearer token returned HTTP status {statusInt} ({status}). We'll have to try again. Content was: {newLine}{content}", (int)response.StatusCode, response.StatusCode, Environment.NewLine, content);
        }

        _cachedResponse = JsonConvert.DeserializeObject<TokenResponse>(content);
        return await GetBearerToken();
    }
}