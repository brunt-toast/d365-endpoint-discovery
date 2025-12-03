using System.CommandLine;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class PostmanCollectionNameOption : Option<string>
{
    public PostmanCollectionNameOption() : base("--postman-collection-name")
    {
        DefaultValueFactory = _ => "Collection";
        Description = "If exporting as a postman collection, set a custom name.";
    }
}
