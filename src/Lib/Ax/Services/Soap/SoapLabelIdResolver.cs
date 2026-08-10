using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal static partial class SoapLabelIdResolver
{
    public static string Resolve(XElement element, bool includeDescendants)
    {
        foreach (var candidate in GetCandidateValues(element, includeDescendants))
        {
            var match = LabelIdRegex().Match(candidate);
            if (match.Success)
            {
                return match.Value;
            }
        }

        return string.Empty;
    }

    private static IEnumerable<string> GetCandidateValues(XElement element, bool includeDescendants)
    {
        foreach (var attribute in element.Attributes())
        {
            yield return attribute.Value;
        }

        var elements = includeDescendants
            ? element.Elements().DescendantsAndSelf()
            : element.Elements();

        foreach (var descendant in elements)
        {
            if (!descendant.HasElements && !string.IsNullOrWhiteSpace(descendant.Value))
            {
                yield return descendant.Value;
            }

            foreach (var attribute in descendant.Attributes())
            {
                yield return attribute.Value;
            }
        }
    }

    [GeneratedRegex(@"@[A-Za-z0-9_]+(?::[A-Za-z0-9_]+)?", RegexOptions.CultureInvariant)]
    private static partial Regex LabelIdRegex();
}
