namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;

internal interface IAxAuthService
{
    Task<string> GetBearerToken();
}