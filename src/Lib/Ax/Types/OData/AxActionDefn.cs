using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

public class AxActionDefn
{
    public required string Name { get; init; }
    public required bool IsBound { get; init; }
    public required AxActionParameterDefn[] Parameters { get; init; }
    public required AxActionReturnTypeDefn? ReturnType { get; init; }

    public static IEnumerable<AxActionDefn> FromODataMetadata(XContainer document)
    {
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";

        foreach (var actionElement in document.Descendants(edm + "Action"))
        {
            var returnTypeElement = actionElement.Element(edm + "ReturnType");
            AxActionReturnTypeDefn? returnType =
                returnTypeElement is null 
                    ? null 
                    : AxActionReturnTypeDefn.Parse(returnTypeElement);

            yield return new AxActionDefn
            {
                Name = (string?)actionElement.Attribute("Name") ?? string.Empty,
                IsBound = (bool?)actionElement.Attribute("IsBound") ?? false,
                Parameters = actionElement.Elements(edm + "Parameter").Select(AxActionParameterDefn.Parse).ToArray(),
                ReturnType = returnType
            };
        }
    }
}