namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

internal sealed class InheritanceResolvedDataContract
{
    public required string Name { get; init; }
    public required IReadOnlyList<AxDataContractPropertyDefn> Properties { get; init; }
}