using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class FormatOption : Option<OutputFormats>
{
    public FormatOption() : base("--format", "-f")
    {
        Description = FormatOptionResources.Description;
    }
}
