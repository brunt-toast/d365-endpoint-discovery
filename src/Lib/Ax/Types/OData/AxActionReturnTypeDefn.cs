using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxActionReturnTypeDefn
{
    public required string TypeName { get; init; }
    public required bool IsNullable { get; init; }

    public static AxActionReturnTypeDefn Parse(XElement returnTypeElement)
    {
        return new AxActionReturnTypeDefn
        {
            TypeName = (string?)returnTypeElement.Attribute("Type") ?? string.Empty,
            IsNullable = (bool?)returnTypeElement.Attribute("Nullable") ?? true
        };
    }
}