using System.Xml.Linq;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Interfaces;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

public record AxDataContractPropertyDefn 
{
    public required string Name { get; init; }
    public required int MinimumOccurances { get; init; }
    public required int? MaximumOccurances { get; init; }
    public required bool IsNullable { get; init; }
    public required string Type { get; init; }
    public bool IsCollection => MaximumOccurances is null or > 1;

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
                MaximumOccurances = ParseMaximumOccurances(element.Attribute("maxOccurs")?.Value),
                IsNullable = element.Attribute("nillable")?.Value == "true",
                Type = ResolveQName(element.Attribute("type")?.Value ?? string.Empty)
            };
        }
    }

    private static int? ParseMaximumOccurances(string? value)
    {
        return value switch
        {
            null => 1,
            "unbounded" => null,
            _ => int.Parse(value)
        };
    }

    private static string ResolveQName(string qname)
    {
        var parts = qname.Split(':');
        return parts.Length == 1 ? parts[0] : parts[1];
    }
}