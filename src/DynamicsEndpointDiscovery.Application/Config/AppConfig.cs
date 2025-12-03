namespace DynamicsEndpointDiscovery.Application.Config;

public class AppConfig 
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string Resource { get; set; }
    public required string TokenRequestEndpoint { get; set; }
}
