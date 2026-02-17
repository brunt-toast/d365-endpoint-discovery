using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Consts;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;

internal class ConfigurableHttpClientFactory : IHttpClientFactory
{
    private readonly HttpClientOptions _opts;
    private readonly ServiceProvider _sp;

    public ConfigurableHttpClientFactory(HttpClientOptions opts)
    {
        _opts = opts;
        IServiceCollection sc = new ServiceCollection();
        sc.AddHttpClient();
        sc.AddHttpClient(HttpClientIdConsts.UserConfigurable)
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var handler = new SocketsHttpHandler();

                if (opts.MaxConnectionsPerServer > 0)
                {
                    handler.MaxConnectionsPerServer = opts.MaxConnectionsPerServer;
                }

                if (opts.AcceptAnySsl)
                {
                    handler.SslOptions.RemoteCertificateValidationCallback = (_, _, _, _) => true;
                }

                return handler;
            });
        _sp = sc.BuildServiceProvider();
    }

    public HttpClient CreateClient(string name)
    {
        if (name == HttpClientIdConsts.UserConfigurable)
        {
            if (_opts.MaxConnectionsPerServer == 0)
            {
                if (_opts.AcceptAnySsl)
                {
                    return new HttpClient(
                        new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        });
                }

                return new HttpClient();
            }
        }

        return _sp.GetRequiredService<IHttpClientFactory>().CreateClient(name);
    }
}

