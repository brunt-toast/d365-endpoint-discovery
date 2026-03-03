using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Validators.AxCredentials;

internal interface IAxCredentialValidator<T> where T : IAxAuthService
{
    IEnumerable<string> ValidateConfig(IAxConfig config);
}