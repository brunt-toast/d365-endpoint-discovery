using System.CommandLine;
using System.CommandLine.Parsing;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Extensions.System.CommandLine;
using Microsoft.Extensions.Localization;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class ResourceOption : Option<string>
{
    private readonly IStringLocalizer<ResourceOptionResources> _localizer;

    public ResourceOption(IStringLocalizer<ResourceOptionResources> localizer) : base("--resource", "-r")
    {
        _localizer = localizer;
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_RESOURCE") ?? string.Empty;
        Validators.Add(NotNullOrWhitespaceValidator);
        Validators.Add(ValidUriValidator);

        Description = _localizer[nameof(ResourceOptionResources.Description),
            "https://*.operations.dynamics.com",
            "https://usnconeboxax1aos.cloud.onebox.dynamics.com/"];
    }

    private void NotNullOrWhitespaceValidator(OptionResult opt)
    {
        if (string.IsNullOrWhiteSpace(opt.GetValue(this)))
        {
            opt.AddError(_localizer[nameof(ResourceOptionResources.ValueMissingError), this.NameAndAliases()]);
        }
    }

    private void ValidUriValidator(OptionResult opt)
    {
        if (!Uri.TryCreate(opt.GetValue(this), UriKind.Absolute, out _))
        {
            opt.AddError(_localizer[nameof(ResourceOptionResources.ValueNotValidUriError), this.NameAndAliases()]);
        }
    }
}