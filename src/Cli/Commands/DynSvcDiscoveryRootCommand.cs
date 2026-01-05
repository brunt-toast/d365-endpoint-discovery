using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Flags;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;
using Microsoft.Extensions.Logging;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;

internal class DynSvcDiscoveryRootCommand : RootCommand
{
    private readonly IMainService _mainService;

    private readonly ClientIdOption _clientIdOption = new();
    private readonly ClientSecretOption _clientSecretOption = new();
    private readonly ResourceOption _resourceOption = new();
    private readonly TokenRequestEndpointOption _tokenRequestEndpointOption = new();
    private readonly GrepGroupsOption _grepGroupsOption = new();
    private readonly GrepServicesOption _grepServicesOption = new();
    private readonly GrepOperationsOption _grepOperationsOption = new();
    private readonly CollectionNameOption _collectionNameOption = new();
    private readonly SchemaOption _schemaOption = new();
    private readonly FormatOption _formatOption = new();

    private readonly MinifyFlag _minifyFlag = new();

    public DynSvcDiscoveryRootCommand(IMainService mainService) : base("Discover Dynamics 365 service endpoints automatically.")
    {
        _mainService = mainService;
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

        Options.Add(_minifyFlag);

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
        OutputSchemas outputSchema = parseResult.GetValue(_schemaOption);
        OutputFormats outputFormat = parseResult.GetValue(_formatOption);
        string collectionName = parseResult.GetValue(_collectionNameOption) ?? string.Empty;
        bool minify = parseResult.GetValue(_minifyFlag);

        string output = await _mainService.GetServiceCollectionAsync(new GetServiceCollectionRequest
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Resource = resource,
            TokenRequestEndpoint = tokenRequestEndpoint,
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
