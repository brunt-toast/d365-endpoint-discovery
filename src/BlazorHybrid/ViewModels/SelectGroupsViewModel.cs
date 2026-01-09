using System.Collections.ObjectModel;
using BlazorHybrid.Extensions.System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Serilog;

namespace BlazorHybrid.ViewModels;

internal class SelectGroupsViewModel : ISelectGroupsViewModel
{
    private readonly IMainService _mainService;
    private readonly ILogger _logger;

    public ObservableCollection<Selectable<DynSvcGroup>> ServiceGroups { get; } = [];
    public string Query { get; set; } = string.Empty;

    public bool SelectAll
    {
        get => ServiceGroups.All(x => x.IsSelected);
        set
        {
            foreach (var group in ServiceGroups)
            {
                group.IsSelected = value;
            }
        }
    }

    public SelectGroupsViewModel(IMainService mainService, ILogger logger)
    {
        _mainService = mainService;
        _logger = logger;
    }

    public async Task InitAsync(ICredentialsViewModel credentials)
    {
        await credentials.SaveAsync();

        try
        {
            var groups = await _mainService.GetAllGroups(new GetAllGroupsRequest
            {
                ClientId = credentials.ClientId,
                ClientSecret = credentials.ClientSecret,
                Resource = credentials.ResourceUri,
                TokenRequestEndpoint = credentials.TokenRequestEndpoint
            });

            ServiceGroups.ReplaceRange(groups.Select(x => new Selectable<DynSvcGroup>(x)));
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while getting groups. {message}", ex.Message);
        }
    }
}

public interface ISelectGroupsViewModel
{
    Task InitAsync(ICredentialsViewModel credentials);
    ObservableCollection<Selectable<DynSvcGroup>> ServiceGroups { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }
}