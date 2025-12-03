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
        Description = "An endpoint from which we can request a Dynamics 365 bearer token. Must be a valid URI. Usually looks like 'https://login.microsoftonline.com/GUID/oauth2/token'";
    }

    private void NotNullOrWhitespaceValidator(OptionResult opt)
    {
        if (string.IsNullOrWhiteSpace(opt.GetValue(this)))
        {
            opt.AddError($"The value for {nameof(TokenRequestEndpointOption)} must be populated.");
        }
    }

    private void ValidUriValidator(OptionResult opt)
    {
        if (!Uri.TryCreate(opt.GetValue(this), UriKind.Absolute, out _))
        {
            opt.AddError($"The value for {nameof(TokenRequestEndpointOption)} must be a valid URI.");
        }
    }
}