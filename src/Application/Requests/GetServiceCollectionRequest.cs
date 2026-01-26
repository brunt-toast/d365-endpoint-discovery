using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;

public class GetServiceCollectionRequest
{
    public required string GrepGroupsRegex { get; init; } 
    public required string GrepServicesRegex { get; init; } 
    public required string GrepOperationsRegex { get; init; } 
    public required string CollectionName { get; init; }
    public required OutputSchemas OutputSchema { get; init; }
    public required OutputFormats OutputFormat { get; init; }
    public required bool Minify { get; init; }
}
