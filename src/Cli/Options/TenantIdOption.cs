using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class TenantIdOption : Option<string>
{
    public TenantIdOption() : base("--tenant-id")
    {
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("DYNAMICS_TENANT_ID") ?? string.Empty;
        Validators.Add(ValidGuidValidator);
        Description = "An Azure Tenant ID. Must be a valid GUID. Required for user authentication flow.";
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
            opt.AddError($"The value for {nameof(TenantIdOption)} must be a valid GUID.");
        }
    }
}
