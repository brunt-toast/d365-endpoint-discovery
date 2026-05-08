using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class CollectionNameOption : Option<string>
{
    public CollectionNameOption() : base("--collection-name")
    {
        DefaultValueFactory = _ => "Collection";
        Description = CollectionNameOptionResources.Description;
    }
}
