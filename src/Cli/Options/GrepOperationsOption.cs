using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class GrepOperationsOption : Option<string>
{
    public GrepOperationsOption() : base("--grep-operations")
    {
        DefaultValueFactory = _ => ".*";
        Description = "Regex filtering for operations.";
    }
}