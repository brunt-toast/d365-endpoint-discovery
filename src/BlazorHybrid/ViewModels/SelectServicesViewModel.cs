using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BlazorHybrid.Extensions.System.Collections.ObjectModel;

namespace BlazorHybrid.ViewModels;

internal class SelectServicesViewModel : ISelectServicesViewModel
{
    private readonly IMainService _mainService;

    public ObservableCollection<Selectable<DynSvc>> Services { get; } = [];
    public string Query { get; set; } = string.Empty;

    public bool SelectAll
    {
        get => Services.All(x => x.IsSelected);
        set
        {
            foreach (var service in Services)
            {
                service.IsSelected = value;
            }
        }
    }

    public SelectServicesViewModel(IMainService mainService)
    {
        _mainService = mainService;
    }

    public async Task InitAsync(ICredentialsViewModel credentials, ISelectGroupsViewModel groups)
    {
        var services = await _mainService.GetServicesForGroups(new GetServicesForGroupsRequest
        {
            ClientId = credentials.ClientId,
            ClientSecret = credentials.ClientSecret,
            Resource = credentials.ResourceUri,
            TokenRequestEndpoint = credentials.TokenRequestEndpoint,
            Groups = groups.ServiceGroups.Where(x => x.IsSelected).Select(x => x.Item).ToArray()
        });

        Services.ReplaceRange(services.Select(x => new Selectable<DynSvc>(x)));
    }

    public void ToggleGroup(IEnumerable<Selectable<DynSvc>> group, bool? isChecked)
    {
        if (isChecked is null)
        {
            return;
        }

        foreach (var service in group)
        {
            service.IsSelected = isChecked == true;
        }
    }
}

public interface ISelectServicesViewModel
{
    Task InitAsync(ICredentialsViewModel credentials, ISelectGroupsViewModel groups);
    ObservableCollection<Selectable<DynSvc>> Services { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }

    void ToggleGroup(IEnumerable<Selectable<DynSvc>> group, bool? isChecked);
}