using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class SchemaOption : Option<OutputSchemas>
{
    public SchemaOption() : base("--schema")
    {
        Description = "Specify the output schema.";
    }
}
