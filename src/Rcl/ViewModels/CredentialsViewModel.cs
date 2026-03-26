using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Extensions.Microsoft.Maui.Storage;
using System.Collections;
using System.Text;
using CommunityToolkit.Maui.Storage;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Types;
using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.ViewModels;

internal class CredentialsViewModel : ICredentialsViewModel
{
    private readonly ISecureStorage _secureStorage;
    private readonly IAxConfig _axConfig;
    private readonly HttpClientOptions _httpClientOptions;
    private readonly IFileSaver _fileSaver;
    private readonly IFilePicker _filePicker;

    private const string AuthKindKey = "AxAuthKind";
    private const string ClientIdKey = "AxClientId";
    private const string ClientSecretKey = "AxClientSecret";
    private const string TokenRequestEndpointKey = "AxTokenRequestEndpoint";
    private const string ResourceUriKey = "AxResourceUri";
    private const string TenantIdKey = "AxTenantId";
    private const string AcceptableThumbprintKey = "AcceptableThumbprint";
    private const string IgnoreSslKey = "IgnoreSsl";
    private const string MaxConnectionsKey = "MaxConnections";

    public AuthKind AuthKind { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string TokenRequestEndpoint { get; set; } = string.Empty;
    public string ResourceUri { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string AcceptableThumbprint { get; set; } = string.Empty;
    public bool CacheCredentials { get; set; }
    public bool IgnoreSsl { get; set; }
    public int MaxConnections { get; set; }

    public CredentialsViewModel(ISecureStorage secureStorage, 
        IAxConfig axConfig, 
        HttpClientOptions httpClientOptions,
        IFileSaver fileSaver,
        IFilePicker filePicker)
    {
        _secureStorage = secureStorage;
        _axConfig = axConfig;
        _httpClientOptions = httpClientOptions;
        _fileSaver = fileSaver;
        _filePicker = filePicker;
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

        AuthKind = await _secureStorage.GetEnumAsync<AuthKind>(AuthKindKey);
        ClientId = await _secureStorage.GetAsync(ClientIdKey) ?? string.Empty;
        ClientSecret = await _secureStorage.GetAsync(ClientSecretKey) ?? string.Empty;
        TokenRequestEndpoint = await _secureStorage.GetAsync(TokenRequestEndpointKey) ?? string.Empty;
        ResourceUri = await _secureStorage.GetAsync(ResourceUriKey) ?? string.Empty;
        TenantId = await _secureStorage.GetAsync(TenantIdKey) ?? string.Empty;
        AcceptableThumbprint = await _secureStorage.GetAsync(AcceptableThumbprintKey) ?? string.Empty;
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

        await _secureStorage.SetEnumAsync(AuthKindKey, AuthKind);
        await _secureStorage.SetStringAsync(ClientIdKey, ClientId);
        await _secureStorage.SetStringAsync(ClientSecretKey, ClientSecret);
        await _secureStorage.SetStringAsync(TokenRequestEndpointKey, TokenRequestEndpoint);
        await _secureStorage.SetStringAsync(ResourceUriKey, ResourceUri);
        await _secureStorage.SetStringAsync(ClientIdKey, ClientId);
        await _secureStorage.SetStringAsync(ClientSecretKey, ClientSecret);
        await _secureStorage.SetStringAsync(TenantIdKey, TenantId);
        await _secureStorage.SetAsync(AcceptableThumbprintKey, AcceptableThumbprint);
        await _secureStorage.SetBoolAsync(IgnoreSslKey, IgnoreSsl);
        await _secureStorage.SetIntAsync(MaxConnectionsKey, MaxConnections);

        _axConfig.ClientId = ClientId;
        _axConfig.ClientSecret = ClientSecret;
        _axConfig.TokenRequestEndpoint = TokenRequestEndpoint;
        _axConfig.Resource = ResourceUri;
        _axConfig.TenantId = TenantId;
        _axConfig.AuthKind = AuthKind;

        _httpClientOptions.AcceptAnySsl = IgnoreSsl;
        _httpClientOptions.MaxConnectionsPerServer = MaxConnections;
        _httpClientOptions.AcceptableThumbprint = AcceptableThumbprint;
    }

    private void ClearCache()
    {
        _secureStorage.Remove(AuthKindKey);
        _secureStorage.Remove(ClientIdKey);
        _secureStorage.Remove(ClientSecretKey);
        _secureStorage.Remove(TokenRequestEndpointKey);
        _secureStorage.Remove(ResourceUriKey);
        _secureStorage.Remove(TenantIdKey);
        _secureStorage.Remove(IgnoreSslKey);
        _secureStorage.Remove(MaxConnectionsKey);
    }

    public void ClearValues()
    {
        AuthKind = default;
        ClientId = string.Empty;
        ClientSecret = string.Empty;
        TokenRequestEndpoint = string.Empty;
        ResourceUri = string.Empty;
        TenantId = string.Empty;
        IgnoreSsl = false;
        MaxConnections = 0;

    }

    public async Task SaveProfileAsync()
    {
        string content = JsonConvert.SerializeObject(new UserAuthConfigurationProfile
        {
            Version = 1,
            ClientId = ClientId,
            Resource = ResourceUri,
            TenantId = TenantId
        });

        using var stream = new MemoryStream(Encoding.Default.GetBytes(content));
        await _fileSaver.SaveAsync("profile.json", stream);
    }

    public async Task LoadProfileAsync()
    {
        FileResult? file = await _filePicker.PickAsync(new PickOptions
        {
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                { DevicePlatform.WinUI, ["json"] }
            })
        });

        if (file is null)
        {
            return;
        }

        string content = await File.ReadAllTextAsync(file.FullPath);
        var config = JsonConvert.DeserializeObject<UserAuthConfigurationProfile>(content);

        if (config is null)
        {
            return;
        }

        ClientId = config.ClientId;
        ResourceUri = config.Resource;
        TenantId = config.TenantId;
    }
}

public interface ICredentialsViewModel
{
    AuthKind AuthKind { get; set; }
    string ClientId { get; set; }
    string ClientSecret { get; set; }
    string TokenRequestEndpoint { get; set; }
    string ResourceUri { get; set; }
    string TenantId { get; set; }
    string AcceptableThumbprint { get; set; }
    bool CacheCredentials { get; set; }
    bool IgnoreSsl { get; set; }
    int MaxConnections { get; set; }

    Task InitAsync();
    void ClearValues();
    Task SaveAsync();
    Task SaveProfileAsync();
    Task LoadProfileAsync();
}