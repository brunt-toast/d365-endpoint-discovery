using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;

public class BuildCustomCollectionRequest
{
    public required OutputSchemas OutputSchema { get; init; }
    public required OutputFormats OutputFormat { get; init; }
    public required string CollectionName { get; init; }
    public required DynSvcGroup[] Services { get; init; }
    public required string Resource { get; init; }
    public required bool Minify { get; init; }
}
