using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxEnumDefn
{
    public required string Name { get; init; }
    public required AxEnumMemberDefn[] Members { get; init; }

    public static IEnumerable<AxEnumDefn> FromODataMetadata(XContainer document)
    {
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";
        return document.Descendants(edm + "EnumType")
            .Select(x => new AxEnumDefn
            {
                Name = (string?)x.Attribute("Name") ?? string.Empty,
                Members = x.Elements(edm + "Member").Select(AxEnumMemberDefn.Parse).ToArray()
            });
    }
}