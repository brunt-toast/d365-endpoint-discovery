using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class MaxConnectionsOption : Option<int>
{
    public MaxConnectionsOption() : base("--max-connections", "-m")
    {
        Description = "Set the maximum number of connections. This helps to avoid socket exhaustion for large jobs, " +
                      "but massively degrades performance (about 12x). '0' (default) represents no limit.";
        DefaultValueFactory = _ => 0;
    }
}
