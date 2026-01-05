using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Flags;

internal class MinifyFlag : Option<bool>
{
    public MinifyFlag() : base("--minify")
    {
        Description = "Remove redundant whitespace in output";
    }
}
