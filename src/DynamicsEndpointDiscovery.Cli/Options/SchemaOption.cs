using System.CommandLine;
using DynamicsEndpointDiscovery.Application.Enums;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class SchemaOption : Option<OutputSchemas>
{
    public SchemaOption() : base("--schema")
    {
        Description = "Specify the output schema.";
    }
}
