using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxEnumMemberDefn
{
    public required string Name { get; init; }
    public required int Value { get; init; }

    public static AxEnumMemberDefn Parse(XElement y)
    {
        return new AxEnumMemberDefn
        {
            Name = (string?)y.Attribute("Name") ?? string.Empty,
            Value = int.Parse((string?)y.Attribute("Value") ?? string.Empty)
        };
    }
}