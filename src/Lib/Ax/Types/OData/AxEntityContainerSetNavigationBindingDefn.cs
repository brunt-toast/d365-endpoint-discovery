using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxEntityContainerSetNavigationBindingDefn
{
    public required string Path { get; init; }
    public required string Target { get; init; }

    public static AxEntityContainerSetNavigationBindingDefn Parse(XElement b)
    {
        return new AxEntityContainerSetNavigationBindingDefn
        {
            Path = (string?)b.Attribute("Path") ?? string.Empty,
            Target = (string?)b.Attribute("Target") ?? string.Empty,
        };
    }
}