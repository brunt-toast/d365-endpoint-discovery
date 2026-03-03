using System.CommandLine;
using System.CommandLine.Parsing;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Extensions.System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class TokenRequestEndpointOption : Option<string>
{
    public TokenRequestEndpointOption(ClientIdOption clientIdOpt) : base("--token-request-endpoint", "-t")
    {
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_TOKEN_REQUEST_ENDPOINT") ?? string.Empty;
        Validators.Add(ValidUriValidator);
        Description = "An endpoint from which we can request a Dynamics 365 bearer token. Must be a valid URI. " +
                      "Usually looks like 'https://login.microsoftonline.com/GUID/oauth2/token', where GUID is usually " +
                      $"the Direcory (tenant) ID of the tenant containing the application described by {clientIdOpt.NameAndAliases()}. " +
                      $"Required for application authentication flow.";
    }

    private void ValidUriValidator(OptionResult opt)
    {
        var value = opt.GetValue(this);
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        if (!Uri.TryCreate(opt.GetValue(this), UriKind.Absolute, out _))
        {
            opt.AddError($"The value for {nameof(TokenRequestEndpointOption)} must be a valid URI.");
        }
    }
}