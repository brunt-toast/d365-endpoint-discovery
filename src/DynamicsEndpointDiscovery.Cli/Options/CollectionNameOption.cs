using System.CommandLine;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class CollectionNameOption : Option<string>
{
    public CollectionNameOption() : base("--collection-name")
    {
        DefaultValueFactory = _ => "Collection";
        Description = "If the output supports collection names, set a custom name.";
    }
}
