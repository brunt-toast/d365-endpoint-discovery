using System.CommandLine;
using System.Text.RegularExpressions;
using DynamicsEndpointDiscovery.Application.Config;
using DynamicsEndpointDiscovery.Application.Services.Dynamics;
using DynamicsEndpointDiscovery.Application.Services.Postman;
using DynamicsEndpointDiscovery.Cli.Flags;
using DynamicsEndpointDiscovery.Cli.Logging;
using DynamicsEndpointDiscovery.Cli.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
    private readonly PostmanCollectionNameOption _postmanCollectionNameOption = new();

    private readonly PostmanFlag _postmanFlag = new();

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
        Options.Add(_postmanCollectionNameOption);

        Options.Add(_postmanFlag);

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
        bool doPostman = parseResult.GetValue(_postmanFlag);
        string postmanCollectionName = parseResult.GetValue(_postmanCollectionNameOption) ?? string.Empty;

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

        if (doPostman)
        {
            var postman = new PostmanCollectionBuilderService().BuildPostmanCollection(services, postmanCollectionName);
            parseResult.InvocationConfiguration.Output.WriteLine(JsonConvert.SerializeObject(postman, Formatting.Indented));
        }
        else
        {
            parseResult.InvocationConfiguration.Output.WriteLine(JsonConvert.SerializeObject(services, Formatting.Indented));
        }

        return 0;
    }
}
