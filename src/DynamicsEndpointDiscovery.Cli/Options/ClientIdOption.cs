using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class ClientIdOption : Option<string>
{
    public ClientIdOption() : base("--client-id", "-c")
    {
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_CLIENT_ID") ?? string.Empty;
        Validators.Add(NotNullOrWhitespaceValidator);
        Validators.Add(ValidGuidValidator);
        Description = "An Azure application (client) ID. Must be a valid GUID.";
    }

    private void NotNullOrWhitespaceValidator(OptionResult opt)
    {
        if (string.IsNullOrWhiteSpace(opt.GetValue(this)))
        {
            opt.AddError($"The value for {nameof(ClientIdOption)} must be populated.");
        }
    }

    private void ValidGuidValidator(OptionResult opt)
    {
        if (!Guid.TryParse(opt.GetValue(this), out _))
        {
            opt.AddError($"The value for {nameof(ClientIdOption)} must be a valid GUID.");
        }
    }
}