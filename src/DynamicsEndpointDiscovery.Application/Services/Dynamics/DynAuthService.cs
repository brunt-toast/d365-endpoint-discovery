using DynamicsEndpointDiscovery.Application.Config;
using DynamicsEndpointDiscovery.Application.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Services.Dynamics;

public class DynAuthService
{
    private readonly AppConfig _config;
    private readonly ILogger _logger;

    private DynTokenResponse? _cachedResponse;

    public DynAuthService(AppConfig config, ILogger logger)
    {
        _config = config;
        _logger = logger;
    }

    public string GetBearerToken()
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
        HttpResponseMessage response = client.Send(request);

        string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("A request for a bearer token returned HTTP status {statusInt} ({status}). We'll have to try again. Content was: {newLine}{content}", (int)response.StatusCode, response.StatusCode, Environment.NewLine, content);
        }

        _cachedResponse = JsonConvert.DeserializeObject<DynTokenResponse>(content);
        return GetBearerToken();
    }
}