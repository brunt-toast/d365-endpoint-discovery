using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxEntityContainerDefn
{
    public required string Name { get; init; }
    public required AxEntityContainerEntitySetDefn[] EntitySets { get; init; }

    public static IEnumerable<AxEntityContainerDefn> FromODataMetadata(XContainer document)
    {
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

        return document.Descendants(edm + "EntityContainer").Select(container =>
            new AxEntityContainerDefn
            {
                Name = (string)container.Attribute("Name")!,
                EntitySets = container.Elements(edm + "EntitySet")
                    .Select(AxEntityContainerEntitySetDefn.Parse)
                    .ToArray()
            });
    }
}