using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BlazorHybrid.Extensions.System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;

namespace BlazorHybrid.ViewModels;

internal class SelectOperationsViewModel : ISelectOperationsViewModel
{
    private readonly IMainService _mainService;

    public ObservableCollection<Selectable<DynSvcOp>> Operations { get; } = [];
    public string Query { get; set; } = string.Empty;

    public bool SelectAll
    {
        get => Operations.All(x => x.IsSelected);
        set
        {
            foreach (var operation in Operations)
            {
                operation.IsSelected = value;
            }
        }
    }

    public SelectOperationsViewModel(IMainService mainService)
    {
        _mainService = mainService;
    }

    public async Task InitAsync(ICredentialsViewModel credentials, ISelectGroupsViewModel groups, ISelectServicesViewModel services)
    {
        var ops = await _mainService.GetOperationsForServices(new GetOperationsForServicesRequest
        {
            Services = services.Services.Where(x => x.IsSelected).Select(x => x.Item).ToArray(),
            ClientId = credentials.ClientId,
            ClientSecret = credentials.ClientSecret,
            Resource = credentials.ResourceUri,
            TokenRequestEndpoint = credentials.TokenRequestEndpoint
        });

        Operations.ReplaceRange(ops.Select(x => new Selectable<DynSvcOp>(x)));
    }


    public void ToggleGroup(IEnumerable<Selectable<DynSvc>> service, bool? isChecked)
    {
        if (isChecked is null)
        {
            return;
        }

        List<string> serviceNames = service.Select(x => x.Item.Name).ToList();
        foreach (var op in Operations.Where(x => serviceNames.Contains(x.Item.ServiceGroupName)))
        {
            op.IsSelected = isChecked == true;
        }
    }

    public void ToggleGroup(IEnumerable<Selectable<DynSvcOp>> group, bool? isChecked)
    {
        foreach (var op in group)
        {
            op.IsSelected = isChecked == true;
        }
    }
}

public interface ISelectOperationsViewModel
{
    Task InitAsync(ICredentialsViewModel credentials, ISelectGroupsViewModel groups, ISelectServicesViewModel services);
    ObservableCollection<Selectable<DynSvcOp>> Operations { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }

    void ToggleGroup(IEnumerable<Selectable<DynSvc>> service, bool? isChecked);
    void ToggleGroup(IEnumerable<Selectable<DynSvcOp>> group, bool? isChecked);
}