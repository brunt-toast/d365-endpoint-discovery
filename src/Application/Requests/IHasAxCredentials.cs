namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;

public interface IHasAxCredentials
{
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
    public string Resource { get; init; }
    public string TokenRequestEndpoint { get; init; }
}