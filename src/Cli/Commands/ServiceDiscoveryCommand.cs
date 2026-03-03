using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Logging.Sinks;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Serilog.Events;
using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;

internal class ServiceDiscoveryCommand : Command
{
    private readonly IMainService _mainService;
    private readonly ICommandParseResultSink _sink;
    private readonly IAxConfig _config;
    private readonly HttpClientOptions _httpClientOptions;

    private readonly ClientIdOption _clientIdOption;
    private readonly ClientSecretOption _clientSecretOption;
    private readonly ResourceOption _resourceOption;
    private readonly TokenRequestEndpointOption _tokenRequestEndpointOption;
    private readonly GrepGroupsOption _grepGroupsOption;
    private readonly GrepServicesOption _grepServicesOption;
    private readonly GrepOperationsOption _grepOperationsOption;
    private readonly CollectionNameOption _collectionNameOption;
    private readonly SchemaOption _schemaOption;
    private readonly FormatOption _formatOption;
    private readonly LogLevelOption _logLevelOption;
    private readonly LogStreamOption _logStreamOption;
    private readonly MaxConnectionsOption _maxConnectionsOption;
    private readonly IgnoreSslOption _ignoreSslOption;
    private readonly MinifyOption _minifyOption;
    private readonly TenantIdOption _tenantIdOption;
    private readonly AuthKindOption _authKindOption;

    public ServiceDiscoveryCommand(IMainService mainService,
        ICommandParseResultSink sink,
        IAxConfig config,
        HttpClientOptions httpClientOptions,
        ClientIdOption clientIdOption,
        ClientSecretOption clientSecretOption,
        ResourceOption resourceOption,
        TokenRequestEndpointOption tokenRequestEndpointOption,
        GrepGroupsOption grepGroupsOption,
        GrepServicesOption grepServicesOption,
        GrepOperationsOption grepOperationsOption,
        CollectionNameOption collectionNameOption,
        SchemaOption schemaOption,
        FormatOption formatOption,
        LogLevelOption logLevelOption,
        LogStreamOption logStreamOption,
        MaxConnectionsOption maxConnectionsOption,
        IgnoreSslOption ignoreSslOption,
        MinifyOption minifyOption,
        TenantIdOption tenantIdOption,
        AuthKindOption authKindOption) 
        : base("service-discovery", "Discover Dynamics 365 service endpoints automatically.")
    {
        _mainService = mainService;
        _sink = sink;
        _config = config;
        _httpClientOptions = httpClientOptions;

        _clientIdOption = clientIdOption;
        _clientSecretOption = clientSecretOption;
        _resourceOption = resourceOption;
        _tokenRequestEndpointOption = tokenRequestEndpointOption;
        _grepGroupsOption = grepGroupsOption;
        _grepServicesOption = grepServicesOption;
        _grepOperationsOption = grepOperationsOption;
        _collectionNameOption = collectionNameOption;
        _schemaOption = schemaOption;
        _formatOption = formatOption;
        _logLevelOption = logLevelOption;
        _logStreamOption = logStreamOption;
        _maxConnectionsOption = maxConnectionsOption;
        _ignoreSslOption = ignoreSslOption;
        _minifyOption = minifyOption;
        _tenantIdOption = tenantIdOption;
        _authKindOption = authKindOption;

        Options.Add(_clientIdOption);
        Options.Add(_clientSecretOption);
        Options.Add(_resourceOption);
        Options.Add(_tokenRequestEndpointOption);
        Options.Add(_grepGroupsOption);
        Options.Add(_grepServicesOption);
        Options.Add(_grepOperationsOption);
        Options.Add(_collectionNameOption);
        Options.Add(_schemaOption);
        Options.Add(_formatOption);
        Options.Add(_logLevelOption);
        Options.Add(_logStreamOption);
        Options.Add(_maxConnectionsOption);
        Options.Add(_ignoreSslOption);
        Options.Add(_minifyOption);
        Options.Add(_tenantIdOption);
        Options.Add(_authKindOption);

        SetAction(ExecuteAction);
    }

    private async Task<int> ExecuteAction(ParseResult parseResult)
    {
        string clientId = parseResult.GetValue(_clientIdOption) ?? string.Empty;
        string clientSecret = parseResult.GetValue(_clientSecretOption) ?? string.Empty;
        string resource = parseResult.GetValue(_resourceOption) ?? string.Empty;
        string tokenRequestEndpoint = parseResult.GetValue(_tokenRequestEndpointOption) ?? string.Empty;
        string grepGroupsRegex = parseResult.GetValue(_grepGroupsOption) ?? string.Empty;
        string grepServicesRegex = parseResult.GetValue(_grepServicesOption) ?? string.Empty;
        string grepOperationsRegex = parseResult.GetValue(_grepOperationsOption) ?? string.Empty;
        string tenantId = parseResult.GetValue(_tenantIdOption) ?? string.Empty;
        OutputSchemas outputSchema = parseResult.GetValue(_schemaOption);
        OutputFormats outputFormat = parseResult.GetValue(_formatOption);
        string collectionName = parseResult.GetValue(_collectionNameOption) ?? string.Empty;
        bool minify = parseResult.GetValue(_minifyOption);
        LogEventLevel logLevel = parseResult.GetValue(_logLevelOption);
        LogDestination logStream = parseResult.GetValue(_logStreamOption);
        int maxConnections = parseResult.GetValue(_maxConnectionsOption);
        bool ignoreSsl = parseResult.GetValue(_ignoreSslOption);
        AuthKind authKind = parseResult.GetValue(_authKindOption);

        using var _ = _sink.Configure(parseResult, logLevel, logStream);

        _httpClientOptions.MaxConnectionsPerServer = maxConnections;
        _httpClientOptions.AcceptAnySsl = ignoreSsl;

        _config.ClientId = clientId;
        _config.ClientSecret = clientSecret;
        _config.Resource = resource;
        _config.TokenRequestEndpoint = tokenRequestEndpoint;
        _config.TenantId = tenantId;
        _config.AuthKind = authKind;

        string output = await _mainService.GetServiceCollectionAsync(new GetServiceCollectionRequest
        {
            CollectionName = collectionName,
            GrepGroupsRegex = grepGroupsRegex,
            GrepOperationsRegex = grepOperationsRegex,
            GrepServicesRegex = grepServicesRegex,
            Minify = minify,
            OutputFormat = outputFormat,
            OutputSchema = outputSchema
        });

        await parseResult.InvocationConfiguration.Output.WriteLineAsync(output);

        return 0;
    }
}
