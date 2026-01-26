using System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Extensions.System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Models;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.ViewModels;

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

    public async Task InitAsync(ISelectGroupsViewModel groups)
    {
        var services = await _mainService.GetServicesForGroups(new GetServicesForGroupsRequest
        {
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
    Task InitAsync(ISelectGroupsViewModel groups);
    ObservableCollection<SelectableDynSvcGroupModel> ServiceGroups { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }
}