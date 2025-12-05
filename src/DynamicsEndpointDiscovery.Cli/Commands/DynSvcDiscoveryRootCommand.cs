using DynamicsEndpointDiscovery.Application.Config;
using DynamicsEndpointDiscovery.Application.Services.Dynamics;
using DynamicsEndpointDiscovery.Cli.Logging;
using DynamicsEndpointDiscovery.Cli.Options;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.Text.RegularExpressions;
using DynamicsEndpointDiscovery.Application.Enums;
using DynamicsEndpointDiscovery.Application.Services;
using DynamicsEndpointDiscovery.Cli.Flags;
using DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

namespace DynamicsEndpointDiscovery.Cli.Commands;

internal class DynSvcDiscoveryRootCommand : RootCommand
{
    private readonly ClientIdOption _clientIdOption = new();
    private readonly ClientSecretOption _clientSecretOption = new();
    private readonly ResourceOption _resourceOption = new();
    private readonly TokenRequestEndpointOption _tokenRequestEndpointOption = new();
    private readonly LogLevelOption _logLevelOption = new();
    private readonly GrepGroupsOption _grepGroupsOption = new();
    private readonly GrepServicesOption _grepServicesOption = new();
    private readonly GrepOperationsOption _grepOperationsOption = new();
    private readonly CollectionNameOption _collectionNameOption = new();
    private readonly SchemaOption _schemaOption = new();
    private readonly FormatOption _formatOption = new();

    private readonly MinifyFlag _minifyFlag = new();

    public DynSvcDiscoveryRootCommand() : base("Discover Dynamics 365 service endpoints automatically.")
    {
        Options.Add(_clientIdOption);
        Options.Add(_clientSecretOption);
        Options.Add(_resourceOption);
        Options.Add(_tokenRequestEndpointOption);
        Options.Add(_logLevelOption);
        Options.Add(_grepGroupsOption);
        Options.Add(_grepServicesOption);
        Options.Add(_grepOperationsOption);
        Options.Add(_collectionNameOption);
        Options.Add(_schemaOption);
        Options.Add(_formatOption);

        Options.Add(_minifyFlag);

        SetAction(ExecuteAction);
    }

    private async Task<int> ExecuteAction(ParseResult parseResult)
    {
        string clientId = parseResult.GetValue(_clientIdOption) ?? string.Empty;
        string clientSecret = parseResult.GetValue(_clientSecretOption) ?? string.Empty;
        string resource = parseResult.GetValue(_resourceOption) ?? string.Empty;
        string tokenRequestEndpoint = parseResult.GetValue(_tokenRequestEndpointOption) ?? string.Empty;
        LogLevel logLevel = parseResult.GetValue(_logLevelOption);
        Regex grepGroupsRegex = new(parseResult.GetValue(_grepGroupsOption) ?? string.Empty);
        Regex grepServicesRegex = new(parseResult.GetValue(_grepServicesOption) ?? string.Empty);
        Regex grepOperationsRegex = new(parseResult.GetValue(_grepOperationsOption) ?? string.Empty);
        OutputSchemas outputSchema = parseResult.GetValue(_schemaOption);
        OutputFormats outputFormat = parseResult.GetValue(_formatOption);
        string collectionName = parseResult.GetValue(_collectionNameOption) ?? string.Empty;
        bool minify = parseResult.GetValue(_minifyFlag);

        var config = new AppConfig
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Resource = resource,
            TokenRequestEndpoint = tokenRequestEndpoint
        };

        var logger = new CommandLogger(parseResult, logLevel);
        var auth = new DynAuthService(config, logger);
        var serviceDiscovery = new DynSvcDiscoveryService(auth, config, logger, grepGroupsRegex, grepServicesRegex, grepOperationsRegex);

        var services = (await serviceDiscovery.MapServicesAsync()).ToArray();

        ICollectionBuilder collectionBuilder = CollectionBuilderFactory.GetCollectionBuilder(outputSchema);
        object data = collectionBuilder.BuildCollection(services, config.Resource, collectionName);
        string serialisation = Serialiser.Serialise(data, outputFormat, minify);

        await parseResult.InvocationConfiguration.Output.WriteLineAsync(serialisation);

        return 0;
    }
}
