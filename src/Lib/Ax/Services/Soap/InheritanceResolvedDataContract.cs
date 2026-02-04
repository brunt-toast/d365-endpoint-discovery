using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal sealed class InheritanceResolvedDataContract
{
    public required string Name { get; init; }
    public required IReadOnlyList<AxDataContractPropertyDefn> Properties { get; init; }
}