using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class TokenRequestEndpointOption : Option<string>
{
    public TokenRequestEndpointOption() : base("--token-request-endpoint", "-t")
    {
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_TOKEN_REQUEST_ENDPOINT") ?? string.Empty;
        Validators.Add(NotNullOrWhitespaceValidator);
        Validators.Add(ValidUriValidator);
        Description = "An endpoint from which we can request a Dynamics 365 bearer token. Must be a valid URI.";
    }

    private void NotNullOrWhitespaceValidator(OptionResult opt)
    {
        if (string.IsNullOrWhiteSpace(opt.GetValue(this)))
        {
            opt.AddError("The value must be populated.");
        }
    }

    private void ValidUriValidator(OptionResult opt)
    {
        if (!Uri.TryCreate(opt.GetValue(this), UriKind.Absolute, out _))
        {
            opt.AddError("The value must be a valid URI.");
        }
    }
}