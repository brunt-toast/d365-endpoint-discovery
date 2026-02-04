using System.Xml.Linq;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Interfaces;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

public class AxDataContractPropertyDefn 
{
    public required string Name { get; init; }
    public required int MinimumOccurances { get; init; }
    public required bool IsNullable { get; init; }
    public required string Type { get; init; }

    public static IEnumerable<AxDataContractPropertyDefn> Parse(XElement document)
    {
        XNamespace xs = "http://www.w3.org/2001/XMLSchema";
        var elements = document.Descendants(xs + "element");
        foreach (var element in elements)
        {
            yield return new AxDataContractPropertyDefn
            {
                Name = element.Attribute("name")?.Value ?? string.Empty,
                MinimumOccurances = int.Parse(element.Attribute("minOccurs")?.Value ?? "0"),
                IsNullable = element.Attribute("nillable")?.Value == "true",
                Type = ResolveQName(element.Attribute("type")?.Value ?? string.Empty)
            };
        }
    }

    private static string ResolveQName(string qname)
    {
        var parts = qname.Split(':');
        return parts.Length == 1 ? parts[0] : parts[1];
    }
}