using System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Extensions.System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Models;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.ViewModels;

internal class SelectOperationsViewModel : ISelectOperationsViewModel
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

    public SelectOperationsViewModel(IMainService mainService)
    {
        _mainService = mainService;
    }

    public async Task InitAsync(ISelectServicesViewModel services)
    {
        var ops = await _mainService.GetOperationsForServices(new GetOperationsForServicesRequest
        {
            Services = services.ServiceGroups.SelectMany(x => x.Children).Where(x => x.IsSelected).Select(x => x.Item).ToArray(),
        });

        var opModels = ops.Select(x => new SelectableDynSvcOpModel(x)).ToArray();

        var serviceModels = opModels
            .GroupBy(x => x.Item.ServiceName)
            .Select(x => new SelectableDynSvcModel(new DynSvc
            {
                ServiceGroupName = x.First().Item.ServiceGroupName,
                Name = x.First().Item.ServiceName,
                Operations = x.Select(y => y.Item).ToArray()
            }, x.ToArray())).ToArray();

        var groupModels = serviceModels
            .GroupBy(x => x.Item.ServiceGroupName)
            .Select(x => new SelectableDynSvcGroupModel(new DynSvcGroup
            {
                Name = x.First().Item.ServiceGroupName,
                Services = x.Select(y => y.Item).ToArray()
            }, x.ToArray()));

        ServiceGroups.ReplaceRange(groupModels);
    }
}

public interface ISelectOperationsViewModel
{
    Task InitAsync(ISelectServicesViewModel services);
    ObservableCollection<SelectableDynSvcGroupModel> ServiceGroups { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }
}