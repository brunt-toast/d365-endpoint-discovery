namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal sealed class TypeNode
{
    public required string TypeName { get; init; }
    public bool IsPrimitive { get; init; }
    //public bool IsNullable { get; init; }
    public Dictionary<string, TypeNode>? Properties { get; init; }
}