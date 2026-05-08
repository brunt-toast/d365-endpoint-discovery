using System.CommandLine;
using System.CommandLine.Parsing;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Extensions.System.CommandLine;
using Microsoft.Extensions.Localization;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class ClientIdOption : Option<string>
{
    private readonly IStringLocalizer<ClientIdOptionResources> _localizer;

    public ClientIdOption(IStringLocalizer<ClientIdOptionResources> localizer) : base("--client-id", "-c")
    {
        _localizer = localizer;
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_CLIENT_ID") ?? string.Empty;
        Validators.Add(NotNullOrWhitespaceValidator);
        Validators.Add(ValidGuidValidator);
        Description = _localizer[nameof(ClientIdOptionResources.Description)];
    }

    private void NotNullOrWhitespaceValidator(OptionResult opt)
    {
        if (string.IsNullOrWhiteSpace(opt.GetValue(this)))
        {
            opt.AddError(_localizer[nameof(ClientIdOptionResources.ValueMissingError), this.NameAndAliases()]);
        }
    }

    private void ValidGuidValidator(OptionResult opt)
    {
        if (!Guid.TryParse(opt.GetValue(this), out _))
        {
            opt.AddError(_localizer[nameof(ClientIdOptionResources.ValueNotValidGuidError), this.NameAndAliases()]);
        }
    }
}