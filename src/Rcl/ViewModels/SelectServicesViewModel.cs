using CommunityToolkit.Mvvm.ComponentModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Extensions.System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Models;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Utils;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.ViewModels;

internal class SelectServicesViewModel : ObservableObject, ISelectServicesViewModel
{
    private readonly IMainService _mainService;

    private readonly ObservableCollection<SelectableDynSvcGroupModel> _allServiceGroups = [];
    public ObservableCollection<SelectableDynSvcGroupModel> VisibleServiceGroups { get; } = [];

    [OnChangedMethod(nameof(OnQueryChanged))]
    public string Query { get; set; } = string.Empty;

    public bool IsLoading { get; set; }

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

    public SelectServicesViewModel(IMainService mainService)
    {
        _mainService = mainService;
    }

    public async Task InitAsync(ISelectGroupsViewModel groups)
    {
        await using var _ = ILoading.UseLoadingAsync(this);

        var services = await _mainService.GetServicesForGroups(new GetServicesForGroupsRequest
        {
            Groups = groups.ServiceGroups.Where(x => x.IsSelected).Select(x => x.Item).ToArray()
        });

        _allServiceGroups.ReplaceRange(services
         .Select(x => new SelectableDynSvcModel(x))
         .GroupBy(x => x.Item.ServiceGroupName)
         .Select(x => new SelectableDynSvcGroupModel(new DynSvcGroup
         {
             Name = x.Key,
             Services = x.Select(y => y.Item).ToArray()
         }, x.Select(y => new SelectableDynSvcModel(y.Item)).ToArray())));

        OnQueryChanged();
    }

    private void OnQueryChanged()
    {
        var filteredServices = _allServiceGroups
            .Select(x => new SelectableDynSvcGroupModel(x.Item,
                x.Children.Where(y => y.FullName.Contains(Query, StringComparison.InvariantCultureIgnoreCase)).ToArray()))
            .Where(x => x.Children.Length > 0);

        VisibleServiceGroups.ReplaceRange(filteredServices);
    }
}

public interface ISelectServicesViewModel : ILoading
{
    Task InitAsync(ISelectGroupsViewModel groups);
    ObservableCollection<SelectableDynSvcGroupModel> VisibleServiceGroups { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }
}