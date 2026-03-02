using System.Net;
using System.Security.Authentication;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Consts;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

internal class AxCallingService
{
    private readonly AxAuthService _authSvc;
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public AxCallingService(AxAuthService authSvc, ILogger logger, IHttpClientFactory httpClientFactory)
    {
        _authSvc = authSvc;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetHttp(string endpoint)
    {
        string bearer = await _authSvc.GetBearerToken();

        HttpRequestMessage request = new(HttpMethod.Get, endpoint);
        request.Headers.Clear();
        request.Headers.Add("Authorization", $"Bearer {bearer}");

        var client = _httpClientFactory.CreateClient(HttpClientIdConsts.UserConfigurable);

        HttpResponseMessage response;
        try
        {
            response = await client.SendAsync(request);
        }
        catch (HttpRequestException ex) when (ex.InnerException is AuthenticationException
                                              {
                                                  Message: "The remote certificate is invalid " +
                                                           "because of errors in the certificate chain: " +
                                                           "UntrustedRoot"
                                              })

        {
            _logger.Error("Couldn't connect to {resource} because the SSL certificate came from an untrusted root. " +
                          "Try skipping SSL validation or importing a certificate. " +
                          "Expect cascading errors from this failure.", endpoint);
            return string.Empty;
        }

        string content = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogError("Too many requests! ({endpoint})", endpoint);
        }
        else if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("A request to {endpoint} returned HTTP status {statusInt} ({status}). Content was: {newLine}{content}", endpoint, (int)response.StatusCode, response.StatusCode, Environment.NewLine, content);
        }

        return content;
    }
}
