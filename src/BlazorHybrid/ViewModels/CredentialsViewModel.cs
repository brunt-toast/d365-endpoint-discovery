using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.ViewModels;

internal class CredentialsViewModel : ICredentialsViewModel
{
    private readonly ISecureStorage _secureStorage;
    private readonly IAxConfig _axConfig;
    private readonly HttpClientOptions _httpClientOptions;

    private const string ClientIdKey = "AxClientId";
    private const string ClientSecretKey = "AxClientSecret";
    private const string TokenRequestEndpointKey = "AxTokenRequestEndpoint";
    private const string ResourceUriKey = "AxResourceUri";
    private const string IgnoreSslKey = "IgnoreSsl";
    private const string MaxConnectionsKey = "MaxConnections";

    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string TokenRequestEndpoint { get; set; } = string.Empty;
    public string ResourceUri { get; set; } = string.Empty;
    public bool CacheCredentials { get; set; }
    public bool IgnoreSsl { get; set; }
    public int MaxConnections { get; set; }

    public CredentialsViewModel(ISecureStorage secureStorage, IAxConfig axConfig, HttpClientOptions httpClientOptions)
    {
        _secureStorage = secureStorage;
        _axConfig = axConfig;
        _httpClientOptions = httpClientOptions;
    }

    public async Task InitAsync()
    {
        if (!string.IsNullOrWhiteSpace(ClientId)
            || !string.IsNullOrWhiteSpace(ClientSecret)
            || !string.IsNullOrWhiteSpace(TokenRequestEndpoint)
            || !string.IsNullOrWhiteSpace(ResourceUri))
        {
            return;
        }

        ClientId = await _secureStorage.GetAsync(ClientIdKey) ?? string.Empty;
        ClientSecret = await _secureStorage.GetAsync(ClientSecretKey) ?? string.Empty;
        TokenRequestEndpoint = await _secureStorage.GetAsync(TokenRequestEndpointKey) ?? string.Empty;
        ResourceUri = await _secureStorage.GetAsync(ResourceUriKey) ?? string.Empty;
        IgnoreSsl = await _secureStorage.GetBoolAsync(IgnoreSslKey);
        MaxConnections = await _secureStorage.GetIntAsync(MaxConnectionsKey);

        if (!string.IsNullOrWhiteSpace(ClientId)
            || !string.IsNullOrWhiteSpace(ClientSecret)
            || !string.IsNullOrWhiteSpace(TokenRequestEndpoint)
            || !string.IsNullOrWhiteSpace(ResourceUri))
        {
            CacheCredentials = true;
        }
    }

    public async Task SaveAsync()
    {
        if (!CacheCredentials)
        {
            ClearCache();
            return;
        }

        await _secureStorage.SetAsync(ClientIdKey, ClientId);
        await _secureStorage.SetAsync(ClientSecretKey, ClientSecret);
        await _secureStorage.SetAsync(TokenRequestEndpointKey, TokenRequestEndpoint);
        await _secureStorage.SetAsync(ResourceUriKey, ResourceUri);
        await _secureStorage.SetAsync(ClientIdKey, ClientId);
        await _secureStorage.SetAsync(ClientSecretKey, ClientSecret);
        await _secureStorage.SetBoolAsync(IgnoreSslKey, IgnoreSsl);
        await _secureStorage.SetIntAsync(MaxConnectionsKey, MaxConnections);

        _axConfig.ClientId = ClientId;
        _axConfig.ClientSecret = ClientSecret;
        _axConfig.TokenRequestEndpoint = TokenRequestEndpoint;
        _axConfig.Resource = ResourceUri;

        _httpClientOptions.AcceptAnySsl = IgnoreSsl;
        _httpClientOptions.MaxConnectionsPerServer = MaxConnections;
    }

    private void ClearCache()
    {
        _secureStorage.Remove(ClientIdKey);
        _secureStorage.Remove(ClientSecretKey);
        _secureStorage.Remove(TokenRequestEndpointKey);
        _secureStorage.Remove(ResourceUriKey);
    }

    public void ClearValues()
    {
        ClientId = string.Empty;
        ClientSecret = string.Empty;
        TokenRequestEndpoint = string.Empty;
        ResourceUri = string.Empty;
    }
}

public interface ICredentialsViewModel
{
    string ClientId { get; set; }
    string ClientSecret { get; set; }
    string TokenRequestEndpoint { get; set; }
    string ResourceUri { get; set; }
    bool CacheCredentials { get; set; }
    bool IgnoreSsl { get; set; }
    int MaxConnections { get; set; }

    Task InitAsync();
    void ClearValues();
    Task SaveAsync();
}