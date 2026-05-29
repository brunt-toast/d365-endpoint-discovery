using System.CommandLine;
using System.CommandLine.Parsing;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Extensions.System.CommandLine;
using Microsoft.Extensions.Localization;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class TenantIdOption : Option<string>
{
    private readonly IStringLocalizer<TenantIdOptionResources> _localizer;

    public TenantIdOption(IStringLocalizer<TenantIdOptionResources> localizer) : base("--tenant-id")
    {
        _localizer = localizer;
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_TENANT_ID") ?? string.Empty;
        Validators.Add(ValidGuidValidator);
        Description = _localizer[nameof(TenantIdOptionResources.Description)];
    }

    private void ValidGuidValidator(OptionResult opt)
    {
        var value = opt.GetValue(this);

        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        if (!Guid.TryParse(value, out _))
        {
            opt.AddError(_localizer[nameof(TenantIdOptionResources.ValueNotValidGuidError), this.NameAndAliases()]);
        }
    }
}
