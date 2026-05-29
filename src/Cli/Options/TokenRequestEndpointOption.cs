using System.CommandLine;
using System.CommandLine.Parsing;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Extensions.System.CommandLine;
using Microsoft.Extensions.Localization;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class TokenRequestEndpointOption : Option<string>
{
    private readonly IStringLocalizer<TokenRequestEndpointOptionResources> _localizer;

    public TokenRequestEndpointOption(ClientIdOption clientIdOpt, 
        IStringLocalizer<TokenRequestEndpointOptionResources> localizer) : base("--token-request-endpoint", "-t")
    {
        _localizer = localizer;
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_TOKEN_REQUEST_ENDPOINT") ?? string.Empty;
        Validators.Add(ValidUriValidator);

        Description = localizer[nameof(TokenRequestEndpointOptionResources.Description),
            "https://login.microsoftonline.com/GUID/oauth2/token",
            clientIdOpt.NameAndAliases()];
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
            opt.AddError(_localizer[nameof(TokenRequestEndpointOptionResources.ValueNotValidUriError), this.NameAndAliases()]);
        }
    }
}