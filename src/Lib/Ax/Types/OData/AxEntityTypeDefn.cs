using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Interfaces;
using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxEntityTypeDefn : ITypeDefn
{
    public required string Name { get; init; }
    public required string[] Keys { get; init; }
    public required IPropertyDefn[] Properties { get; init; }
    public required AxEntityTypeAnnotationDefn[] Annotations { get; init; }

    public Dictionary<string, object> GetDefault()
    {
        return Properties.Select(x => new KeyValuePair<string, object>(x.Name, x.Type switch
        {
            "array" => Array.Empty<object>(),
            "number" => int.MaxValue,
            "boolean" => false,
            "string" => string.Empty,

            _ => $"[Unknown type {x.Type}]"
        })).DistinctBy(x => x.Key).ToDictionary();
    }

    public static IEnumerable<AxEntityTypeDefn> FromODataMetadata(XContainer document)
    {
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

        return document.Descendants(edm + "EntityType").Select(entityElement => new AxEntityTypeDefn
        {
            Name = (string)entityElement.Attribute("Name")!,
            Keys = entityElement.Element(edm + "Key")?.Elements(edm + "PropertyRef")
                .Select(e => (string)e.Attribute("Name")!).ToArray() ?? [],
            Properties = entityElement.Elements(edm + "Property").Select(AxEntityTypePropertyDefn.Parse).ToArray<IPropertyDefn>(),
            Annotations = entityElement.Elements(edm + "Annotation").Select(AxEntityTypeAnnotationDefn.Parse).ToArray()
        });
    }
}