using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class ClientSecretOption : Option<string>
{
    public ClientSecretOption() : base("--client-secret", "-s")
    {
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_CLIENT_SECRET") ?? string.Empty;
        Validators.Add(NotNullOrWhitespaceValidator);
        Description = "An Azure application client secret for the application described by the client ID.";
    }

    private void NotNullOrWhitespaceValidator(OptionResult opt)
    {
        if (string.IsNullOrWhiteSpace(opt.GetValue(this)))
        {
            opt.AddError($"The value for {nameof(ClientSecretOption)} must be populated.");
        }
    }
}