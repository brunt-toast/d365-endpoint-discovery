using System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Extensions.System.Collections.ObjectModel;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Models;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.ViewModels;

internal class SelectGroupsViewModel : ISelectGroupsViewModel
{
    private readonly IMainService _mainService;
    private readonly ILogger _logger;

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
            var groups = await _mainService.GetAllGroups();
            ServiceGroups.ReplaceRange(groups.Select(x => new SelectableDynSvcGroupModel(x)));
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
    ObservableCollection<SelectableDynSvcGroupModel> ServiceGroups { get; }
    string Query { get; set; }
    bool SelectAll { get; set; }
}