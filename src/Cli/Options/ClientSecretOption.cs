using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class ClientSecretOption : Option<string>
{
    public ClientSecretOption() : base("--client-secret", "-s")
    {
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_CLIENT_SECRET") ?? string.Empty;
        Description = "An Azure application client secret for the application described by the client ID. " +
                      "Required for application authentication flows.";
    }
}