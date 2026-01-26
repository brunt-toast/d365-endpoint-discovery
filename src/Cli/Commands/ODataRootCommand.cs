using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Logging.Sinks;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;
using Serilog.Events;
using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;

internal class ODataCommand : Command
{
    private readonly IAxODataService _oDataService;
    private readonly ICommandParseResultSink _sink;
    private readonly IAxConfig _config;
    private readonly ClientIdOption _clientIdOption = new();
    private readonly ClientSecretOption _clientSecretOption = new();
    private readonly ResourceOption _resourceOption = new();
    private readonly TokenRequestEndpointOption _tokenRequestEndpointOption = new();
    private readonly LogLevelOption _logLevelOption = new();

    public ODataCommand(IAxODataService oDataService,
        ICommandParseResultSink sink,
        IAxConfig config) : base("odata", "Get OData metadata for the instance")
    {
        _oDataService = oDataService;
        _sink = sink;
        _config = config;

        Options.Add(_clientIdOption);
        Options.Add(_clientSecretOption);
        Options.Add(_resourceOption);
        Options.Add(_tokenRequestEndpointOption);
        Options.Add(_logLevelOption);

        SetAction(ExecuteAction);
    }

    private async Task<int> ExecuteAction(ParseResult parseResult)
    {
        string clientId = parseResult.GetValue(_clientIdOption) ?? string.Empty;
        string clientSecret = parseResult.GetValue(_clientSecretOption) ?? string.Empty;
        string resource = parseResult.GetValue(_resourceOption) ?? string.Empty;
        string tokenRequestEndpoint = parseResult.GetValue(_tokenRequestEndpointOption) ?? string.Empty;
        LogEventLevel logLevel = parseResult.GetValue(_logLevelOption);

        using var _ = _sink.Configure(parseResult, logLevel);

        _config.ClientId = clientId;
        _config.ClientSecret = clientSecret;
        _config.Resource = resource;
        _config.TokenRequestEndpoint = tokenRequestEndpoint;

        var output = await _oDataService.GetRawMetadata();
        await parseResult.InvocationConfiguration.Output.WriteLineAsync(output);

        return 0;
    }
}
