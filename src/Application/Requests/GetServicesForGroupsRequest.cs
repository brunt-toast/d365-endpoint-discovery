using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;

public class GetServicesForGroupsRequest : IHasAxCredentials
{
    public required DynSvcGroup[] Groups { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string Resource { get; init; }
    public required string TokenRequestEndpoint { get; init; }
}