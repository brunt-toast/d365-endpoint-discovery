namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Interfaces;

public interface ITypeDefn
{
    string Name { get; }
    IPropertyDefn[] Properties { get; }

    Dictionary<string, object> GetDefault();
}