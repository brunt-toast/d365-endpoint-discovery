namespace DynamicsEndpointDiscovery.Cli.Flags;

internal class PostmanFlag : Flag
{
    public PostmanFlag() : base("--postman")
    {
        Description = "Output as a postman collection.";
    }
}
