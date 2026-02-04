using CommunityToolkit.Mvvm.ComponentModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Extensions.System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Models;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.ViewModels;

internal class SelectOperationsViewModel : ObservableObject, ISelectOperationsViewModel
{
    private readonly IMainService _mainService;

    private readonly ObservableCollection<SelectableDynSvcGroupModel> _allServiceGroups = [];
    public ObservableCollection<SelectableDynSvcGroupModel> VisibleServiceGroups { get; } = [];

    [OnChangedMethod(nameof(OnQueryChanged))]
    public string Query { get; set; } = string.Empty;
    
    public bool SelectAll
    {
        get => VisibleServiceGroups.All(x => x.IsSelected);
        set
        {
            foreach (var group in VisibleServiceGroups)
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
            Services = services.VisibleServiceGroups.SelectMany(x => x.Children).Where(x => x.IsSelected).Select(x => x.Item).ToArray(),
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

        _allServiceGroups.ReplaceRange(groupModels);

        OnQueryChanged();
    }

    private void OnQueryChanged()
    {
        var filteredServices = _allServiceGroups
            .Select(x => new SelectableDynSvcGroupModel(x.Item,
                x.Children.Select(y => new SelectableDynSvcModel(y.Item,
                    y.Children.Where(z => z.FullName.Contains(Query, StringComparison.InvariantCultureIgnoreCase))
                        .ToArray())).Where(y => y.Children.Length > 0)
                    .ToArray())).Where(x => x.Children.Length > 0);

        VisibleServiceGroups.ReplaceRange(filteredServices);
    }
}

public interface ISelectOperationsViewModel
{
    Task InitAsync(ISelectServicesViewModel services);
    ObservableCollection<SelectableDynSvcGroupModel> VisibleServiceGroups { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }
}