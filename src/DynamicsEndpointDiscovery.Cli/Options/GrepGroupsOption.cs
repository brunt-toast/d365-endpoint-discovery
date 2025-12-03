using System.CommandLine;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class GrepGroupsOption : Option<string>
{
    public GrepGroupsOption() : base("--grep-groups")
    {
        DefaultValueFactory = _ => ".*";
        Description = "Regex filtering for groups.";
    }
}