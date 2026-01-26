using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxEntityTypeDefn
{
    public required string Name { get; init; }
    public required string[] Keys { get; init; }
    public required AxEntityTypePropertyDefn[] Properties { get; init; }
    public required AxEntityTypeAnnotationDefn[] Annotations { get; init; }

    public static IEnumerable<AxEntityTypeDefn> FromODataMetadata(XContainer document)
    {
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

        return document.Descendants(edm + "EntityType").Select(entityElement => new AxEntityTypeDefn
        {
            Name = (string)entityElement.Attribute("Name")!,
            Keys = entityElement.Element(edm + "Key")?.Elements(edm + "PropertyRef")
                .Select(e => (string)e.Attribute("Name")!).ToArray() ?? [],
            Properties = entityElement.Elements(edm + "Property").Select(AxEntityTypePropertyDefn.Parse).ToArray(),
            Annotations = entityElement.Elements(edm + "Annotation").Select(AxEntityTypeAnnotationDefn.Parse).ToArray()
        });
    }
}