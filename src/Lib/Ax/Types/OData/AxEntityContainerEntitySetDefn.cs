using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxEntityContainerEntitySetDefn
{
    public required string Name { get; init; }
    public required string EntityTypeName { get; init; }
    public required AxEntityContainerSetNavigationBindingDefn[] NavigationBindings { get; init; }

    public static AxEntityContainerEntitySetDefn Parse(XElement set)
    {
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

        return new AxEntityContainerEntitySetDefn
        {
            Name = (string)set.Attribute("Name")!,
            EntityTypeName = (string)set.Attribute("EntityType")!,
            NavigationBindings = set.Elements(edm + "NavigationPropertyBinding")
                .Select(AxEntityContainerSetNavigationBindingDefn.Parse)
                .ToArray()
        };
    }
}