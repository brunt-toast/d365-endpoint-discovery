using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.ViewModels;

internal class ConnectionOptionsViewModel : IConnectionOptionsViewModel
{
    private readonly HttpClientOptions _httpClientOptions;

    public string AcceptableThumbprint { get; set; } = string.Empty;
    public int MaxConnections { get; set; }
    public bool IgnoreSsl { get; set; }

    public ConnectionOptionsViewModel(HttpClientOptions httpClientOptions)
    {
        _httpClientOptions = httpClientOptions;
    }
    public void Save()
    {
        _httpClientOptions.AcceptAnySsl = IgnoreSsl;
        _httpClientOptions.MaxConnectionsPerServer = MaxConnections;
        _httpClientOptions.AcceptableThumbprint = AcceptableThumbprint;
    }
}

public interface IConnectionOptionsViewModel
{
    string AcceptableThumbprint { get; set; }
    int MaxConnections { get; set; }
    bool IgnoreSsl { get; set; }

    void Save();
}