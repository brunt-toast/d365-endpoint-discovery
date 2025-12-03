using System.CommandLine;
using System.CommandLine.Parsing;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class ResourceOption : Option<string>
{
    public ResourceOption() : base("--resource", "-r")
    {
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_RESOURCE") ?? string.Empty;
        Validators.Add(NotNullOrWhitespaceValidator);
        Validators.Add(ValidUriValidator);
        Description = "A Dynamics 365 instance. Must be a valid URI. Usually looks like 'https://*.operations.dynamics.com'.";
    }

    private void NotNullOrWhitespaceValidator(OptionResult opt)
    {
        if (string.IsNullOrWhiteSpace(opt.GetValue(this)))
        {
            opt.AddError($"The value for {nameof(ResourceOption)} must be populated.");
        }
    }

    private void ValidUriValidator(OptionResult opt)
    {
        if (!Uri.TryCreate(opt.GetValue(this), UriKind.Absolute, out _))
        {
            opt.AddError($"The value for {nameof(ResourceOption)} must be a valid URI.");
        }
    }
}