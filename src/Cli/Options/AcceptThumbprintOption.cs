using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class AcceptThumbprintOption : Option<string>
{
    public AcceptThumbprintOption() : base("--accept-thumbprint")
    {
        Description = "Acceptable thumbprint for X.509 certificate when validating SSL connections. " +
                      "This will be considered valid in addition to any certificates considered valid by the system.";
    }
}