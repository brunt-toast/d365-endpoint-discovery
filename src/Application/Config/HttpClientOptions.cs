namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;

public class HttpClientOptions
{
    public int MaxConnectionsPerServer { get; set; }
    public bool AcceptAnySsl { get; set; }
    public string AcceptableThumbprint { get; set; } = string.Empty;
}
