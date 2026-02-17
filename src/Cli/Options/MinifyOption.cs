using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class MinifyOption : Option<bool>
{
    public MinifyOption() : base("--minify")
    {
        Description = "Remove redundant whitespace in output";
    }
}
