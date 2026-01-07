using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class GrepServicesOption : Option<string>
{
    public GrepServicesOption() : base("--grep-services")
    {
        DefaultValueFactory = _ => ".*";
        Description = "Regex filtering for services.";
    }
}