using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Validators.AxCredentials;

internal class ApplicationAxCredentialValidator : IAxCredentialValidator<ApplicationAxAuthService>
{
    public IEnumerable<string> ValidateConfig(IAxConfig config)
    {
        if (!Uri.TryCreate(config.Resource, UriKind.Absolute, out _))
        {
            yield return $"{nameof(config.Resource)}: Expected a valid URI, but got \"{config.Resource}\"";
        }

        if (!Uri.TryCreate(config.TokenRequestEndpoint, UriKind.Absolute, out _))
        {
            yield return $"{nameof(config.TokenRequestEndpoint)}: Expected a valid URI, but got \"{config.TokenRequestEndpoint}\"";
        }

        if (!Guid.TryParse(config.ClientId, out _))
        {
            yield return $"{nameof(config.ClientId)}: Expected a valid URI, GUID got \"{config.ClientId}\"";
        }

        if (string.IsNullOrWhiteSpace(config.ClientSecret))
        {
            yield return $"{nameof(config.ClientSecret)}: Expected a value, but got \"{config.ClientSecret}\"";
        }
    }
}