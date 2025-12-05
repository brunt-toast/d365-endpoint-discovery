using System.CommandLine;
using DynamicsEndpointDiscovery.Application.Enums;
using DynamicsEndpointDiscovery.Cli.Enums;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class FormatOption : Option<OutputFormats>
{
    public FormatOption() : base("--format", "-f")
    {
        Description = "Data format to output.";
    }
}
