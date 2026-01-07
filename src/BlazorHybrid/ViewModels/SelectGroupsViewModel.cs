using System.Collections.ObjectModel;
using BlazorHybrid.Extensions.System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace BlazorHybrid.ViewModels;

internal class SelectGroupsViewModel : ISelectGroupsViewModel
{
    private readonly IMainService _mainService;

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

    public SelectGroupsViewModel(IMainService mainService)
    {
        _mainService = mainService;
    }

    public async Task InitAsync(ICredentialsViewModel credentials)
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
}

public interface ISelectGroupsViewModel
{
    Task InitAsync(ICredentialsViewModel credentials);
    ObservableCollection<Selectable<DynSvcGroup>> ServiceGroups { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }
}