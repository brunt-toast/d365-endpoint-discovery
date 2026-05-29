using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Config;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.ViewModels;

internal class ConnectionOptionsViewModel : IConnectionOptionsViewModel
{
    private readonly HttpClientOptions _httpClientOptions;
    private readonly AppSettings _settings;

    public string AcceptableThumbprint { get; set; } = string.Empty;
    public int MaxConnections { get; set; }
    public bool IgnoreSsl { get; set; }
    public bool UseMock { get; set; }

    public ConnectionOptionsViewModel(HttpClientOptions httpClientOptions, AppSettings settings)
    {
        _httpClientOptions = httpClientOptions;
        _settings = settings;
    }

    public void Init()
    {
        _settings.Load();
        UseMock = _settings.UseMock;
    }

    public void Save()
    {
        _httpClientOptions.AcceptAnySsl = IgnoreSsl;
        _httpClientOptions.MaxConnectionsPerServer = MaxConnections;
        _httpClientOptions.AcceptableThumbprint = AcceptableThumbprint;

        _settings.UseMock = UseMock;
        _settings.Save();
    }
}

public interface IConnectionOptionsViewModel
{
    string AcceptableThumbprint { get; set; }
    int MaxConnections { get; set; }
    bool IgnoreSsl { get; set; }
    bool UseMock { get; set; }

    void Init();
    void Save();
}