using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxEntityTypeAnnotationDefn
{
    public required string Term { get; init; }
    public required string? String { get; init; }
    public required bool? Bool { get; init; }
    public required string? EnumMember { get; init; }

    public static AxEntityTypeAnnotationDefn Parse(XElement ann)
    {
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

        return new AxEntityTypeAnnotationDefn
        {
            Term = (string)ann.Attribute("Term")!,

            String = (string?)ann.Attribute("String"),
            Bool = (bool?)ann.Attribute("Bool"),

            EnumMember =
                ann.Element(edm + "EnumMember")?.Value
        };
    }
}