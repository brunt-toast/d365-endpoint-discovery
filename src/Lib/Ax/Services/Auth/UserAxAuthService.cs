using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;

internal class UserAxAuthService : IAxAuthService
{
    private readonly IAxConfig _axConfig;

    public UserAxAuthService(IAxConfig axConfig)
    {
        _axConfig = axConfig;
    }

    private AuthenticationResult? _cache;
    public async Task<string> GetBearerToken()
    {
        var builder = PublicClientApplicationBuilder.Create(_axConfig.ClientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, _axConfig.TenantId)
            .WithDefaultRedirectUri();
        var authenticationClient = builder.Build();

        var storageProps = new StorageCreationPropertiesBuilder("msal_cache.dat", MsalCacheHelper.UserRootDirectory).Build();
        var cacheHelper = MsalCacheHelper.CreateAsync(storageProps).GetAwaiter().GetResult();
        cacheHelper.RegisterCache(authenticationClient.UserTokenCache);

        if (_cache is not null && _cache.ExpiresOn > DateTime.Now)
        {
            return _cache.AccessToken;
        }

        _cache = await authenticationClient.AcquireTokenInteractive([$"{_axConfig.Resource}/.default"]).ExecuteAsync();
        return await GetBearerToken();
    }
}
