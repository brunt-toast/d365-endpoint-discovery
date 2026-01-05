namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;

public class AxConfig : IAxConfig
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string Resource { get; set; }
    public required string TokenRequestEndpoint { get; set; }
}

public interface IAxConfig
{
    string ClientId { get; set; }
    string ClientSecret { get; set; }
    string Resource { get; set; }
    string TokenRequestEndpoint { get; set; }
}