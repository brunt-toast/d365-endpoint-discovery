namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Interfaces;

public interface IPropertyDefn
{
    string Name { get; }
    string Type { get; }
    bool IsNullable { get; }
}