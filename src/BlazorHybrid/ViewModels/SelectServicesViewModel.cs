using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BlazorHybrid.Extensions.System.Collections.ObjectModel;
using BlazorHybrid.Models;

namespace BlazorHybrid.ViewModels;

internal class SelectServicesViewModel : ISelectServicesViewModel
{
    private readonly IMainService _mainService;

    public ObservableCollection<SelectableDynSvcGroupModel> ServiceGroups { get; } = [];
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

        ServiceGroups.ReplaceRange(services
         .Select(x => new SelectableDynSvcModel(x))
         .GroupBy(x => x.Item.ServiceGroupName)
         .Select(x => new SelectableDynSvcGroupModel(new DynSvcGroup
         {
             Name = x.Key,
             Services = x.Select(y => y.Item).ToArray()
         }, x.Select(y => new SelectableDynSvcModel(y.Item)).ToArray())));
    }
}

public interface ISelectServicesViewModel
{
    Task InitAsync(ICredentialsViewModel credentials, ISelectGroupsViewModel groups);
    ObservableCollection<SelectableDynSvcGroupModel> ServiceGroups { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }
}