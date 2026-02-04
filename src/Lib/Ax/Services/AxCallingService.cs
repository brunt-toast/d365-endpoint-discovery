using System.Net;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

internal class AxCallingService
{
    private readonly AxAuthService _authSvc;
    private readonly ILogger _logger;

    public AxCallingService(AxAuthService authSvc, ILogger logger)
    {
        _authSvc = authSvc;
        _logger = logger;
    }

    public async Task<string> GetHttp(string endpoint)
    {
        string bearer = await _authSvc.GetBearerToken();
        using var client = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

        HttpRequestMessage request = new(HttpMethod.Get, endpoint);
        request.Headers.Clear();
        request.Headers.Add("Authorization", $"Bearer {bearer}");
        var response = await client.SendAsync(request);

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
