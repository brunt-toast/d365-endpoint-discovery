using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class ClientSecretOption : Option<string>
{
    public ClientSecretOption() : base("--client-secret", "-s")
    {
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_CLIENT_SECRET") ?? string.Empty;
        Description = ClientSecretOptionResources.Description;
    }
}