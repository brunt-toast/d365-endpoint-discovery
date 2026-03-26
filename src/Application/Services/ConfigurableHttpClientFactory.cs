using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Consts;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;

internal class ConfigurableHttpClientFactory : IHttpClientFactory
{
    private readonly ILogger _logger;
    private readonly HttpClientOptions _opts;
    private readonly ServiceProvider _sp;

    public ConfigurableHttpClientFactory(HttpClientOptions opts, ILogger logger)
    {
        _opts = opts;
        _logger = logger;
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

                handler.SslOptions.RemoteCertificateValidationCallback = SslOptionsRemoteCertificateValidationCallback;

                return handler;
            });
        _sp = sc.BuildServiceProvider();
    }

    public HttpClient CreateClient(string name)
    {
        return _sp.GetRequiredService<IHttpClientFactory>().CreateClient(name);
    }

    private bool SslOptionsRemoteCertificateValidationCallback(object o, X509Certificate? x509Cert, X509Chain? x509Chain, SslPolicyErrors sslErrors)
    {
        if (_opts.AcceptAnySsl)
        {
            return true;
        }

        if (!string.IsNullOrWhiteSpace(_opts.AcceptableThumbprint)
            && x509Cert is X509Certificate2 x509Cert2
            && x509Cert2.Thumbprint.Equals(_opts.AcceptableThumbprint, StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        return sslErrors == SslPolicyErrors.None;
    }
}

