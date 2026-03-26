using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class AcceptThumbprintOption : Option<string>
{
    public AcceptThumbprintOption() : base("--accept-thumbprint")
    {
        Description = "Acceptable thumbprint for X.509 certificate when validating SSL connections. " +
                      "If unset, certificates trusted by the system will be considered valid.";
    }
}