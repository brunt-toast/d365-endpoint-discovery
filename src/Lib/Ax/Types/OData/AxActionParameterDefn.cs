using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxActionParameterDefn
{
    public required string Name { get; init; }
    public required string TypeName { get; init; }

    public static AxActionParameterDefn Parse(XElement p)
    {
        return new AxActionParameterDefn
        {
            Name = (string?)p.Attribute("Name") ?? string.Empty,
            TypeName = (string?)p.Attribute("Type") ?? string.Empty,
        };
    }
}