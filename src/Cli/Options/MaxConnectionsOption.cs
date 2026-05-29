using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class MaxConnectionsOption : Option<int>
{
    public MaxConnectionsOption() : base("--max-connections", "-m")
    {
        Description = MaxConnectionsOptionResources.Description;
        DefaultValueFactory = _ => 0;
    }
}
