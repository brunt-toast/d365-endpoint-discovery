using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxEntityTypePropertyDefn
{
    public required string Name { get; init; }
    public required string TypeName { get; init; }
    public required bool IsNullable { get; init; }
    public required string? AxType { get; init; }
    public required AxEntityTypeAnnotationDefn[] Annotations { get; init; }

    public static AxEntityTypePropertyDefn Parse(XElement prop)
    {
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

        var annotations =
            prop.Elements(edm + "Annotation")
                .Select(AxEntityTypeAnnotationDefn.Parse)
                .ToDictionary(a => a.Term);

        var axType =
            annotations.TryGetValue(
                "Microsoft.Dynamics.OData.Core.V1.AXType",
                out var axAnn)
                ? axAnn.EnumMember?.Split('/').Last()
                : null;

        return new AxEntityTypePropertyDefn
        {
            Name = (string)prop.Attribute("Name")!,
            TypeName = (string)prop.Attribute("Type")!,
            IsNullable = (bool?)prop.Attribute("Nullable") ?? true,
            AxType = axType,
            Annotations = annotations.Select(x => x.Value).ToArray()
        };
    }
}