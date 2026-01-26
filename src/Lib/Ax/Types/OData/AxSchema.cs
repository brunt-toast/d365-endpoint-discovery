using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxSchema
{
    public required AxEnumDefn[] EnumTypes { get; init; }
    public required AxEntityTypeDefn[] EntityTypes { get; init; }
    public required AxActionDefn[] Actions { get; init; }
    public required AxEntityContainerDefn[] EntityContainers { get; init; }

    public static IEnumerable<AxSchema> FromODataMetadata(XContainer document)
    {
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

        return document.Descendants(edm + "Schema").Select(x => new AxSchema
        {
            EnumTypes = AxEnumDefn.FromODataMetadata(x).ToArray(),
            EntityTypes = AxEntityTypeDefn.FromODataMetadata(x).ToArray(),
            Actions = AxActionDefn.FromODataMetadata(x).ToArray(),
            EntityContainers = AxEntityContainerDefn.FromODataMetadata(x).ToArray()
        });
    }
}
