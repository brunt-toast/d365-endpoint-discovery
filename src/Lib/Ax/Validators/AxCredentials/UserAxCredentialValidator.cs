using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Validators.AxCredentials;

internal class UserAxCredentialValidator : IAxCredentialValidator<UserAxAuthService>
{
    public IEnumerable<string> ValidateConfig(IAxConfig config)
    {
        if (!Uri.TryCreate(config.Resource, UriKind.Absolute, out _))
        {
            yield return $"{nameof(config.Resource)}: Expected a valid URI, but got \"{config.Resource}\"";
        }

        if (!Guid.TryParse(config.ClientId, out _))
        {
            yield return $"{nameof(config.ClientId)}: Expected a valid URI, GUID got \"{config.ClientId}\"";
        }

        if (!Guid.TryParse(config.TenantId, out _))
        {
            yield return $"{nameof(config.TenantId)}: Expected a valid URI, GUID got \"{config.TenantId}\"";
        }
    }
}