namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Types;

internal class UserAuthConfigurationProfile
{
    public required int Version { get; init; } 
    public required string ClientId { get; init; } = string.Empty;
    public required string Resource { get; init; } = string.Empty;
    public required string TenantId { get; init; } = string.Empty;
}
