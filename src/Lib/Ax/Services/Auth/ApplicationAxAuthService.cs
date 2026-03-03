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

    public ApplicationAxAuthService(IAxConfig config, ILogger logger, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetBearerToken()
    {
        if (_cachedResponse is not null && DateTimeOffset.FromUnixTimeSeconds(_cachedResponse.ExpiresOn) >= DateTimeOffset.Now)
        {
            return _cachedResponse.AccessToken;
        }

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
            return string.Empty;
        }

        _cachedResponse = JsonConvert.DeserializeObject<TokenResponse>(content);
        return await GetBearerToken();
    }
}