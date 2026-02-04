using System.Xml.Linq;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Interfaces;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;


public class AxDataContractDefn 
{
    public required string Name { get; init; }
    public required AxDataContractPropertyDefn[] Properties { get; init; }
    public required string Extends { get; init; }

    public static AxDataContractDefn Parse(XElement document)
    {
        XNamespace xs = "http://www.w3.org/2001/XMLSchema";

        var extension = document.Descendants(xs + "extension").FirstOrDefault();

        return new AxDataContractDefn
        {
            Name = document.Attribute("name")?.Value ?? string.Empty,
            Properties = AxDataContractPropertyDefn.Parse(document).ToArray(),
            Extends = extension is null
                ? string.Empty
                : ResolveQName(extension.Attribute("base")!.Value)
        };
    }

    private static string ResolveQName(string qname)
    {
        var parts = qname.Split(':');
        return parts.Length == 1 ? parts[0] : parts[1];
    }
}