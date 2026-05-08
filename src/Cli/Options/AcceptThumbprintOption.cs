using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class AcceptThumbprintOption : Option<string>
{
    public AcceptThumbprintOption() : base("--accept-thumbprint")
    {
        Description = AcceptThumbprintOptionResources.Description;
    }
}