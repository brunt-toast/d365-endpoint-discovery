using CommunityToolkit.Maui.Storage;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Models;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Utils;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.ViewModels;

internal class BuildCollectionViewModel : IBuildCollectionViewModel
{
    private readonly IMainService _mainService;
    private readonly IFileSaver _fileSaver;
    private readonly ILauncher _launcher;
    private readonly Dictionary<OutputSchemas, ICollectionBuilderOptions> _options;

    private string _resource = string.Empty;
    private DynSvcGroup[] _services = [];
    private OutputSchemas _outputSchema;

    public OutputSchemas[] AvailableOutputSchemas { get; } = Enum.GetValues<OutputSchemas>();

    public string CollectionName { get; set; } = "Collection";
    public OutputSchemas OutputSchema
    {
        get => _outputSchema;
        set
        {
            _outputSchema = value;
            CurrentOptions = _options[value];
            CurrentOptions.Validate();
        }
    }

    public ICollectionBuilderOptions CurrentOptions { get; private set; }

    public string OutputPath { get; private set; } = string.Empty;
    public bool IsLoading { get; set; }

    public BuildCollectionViewModel(IMainService mainService, IFileSaver fileSaver, ILauncher launcher)
    {
        _mainService = mainService;
        _fileSaver = fileSaver;
        _launcher = launcher;

        _options = AvailableOutputSchemas.ToDictionary(x => x, CollectionBuilderOptionsFactory.Create);
        CurrentOptions = _options[OutputSchemas.Postman];
        OutputSchema = OutputSchemas.Postman;
    }

    public void Init(ICredentialsViewModel credentials, ISelectOperationsViewModel operations)
    {
        using var _ = ILoading.UseLoading(this);

        _resource = credentials.ResourceUri;

        var targetedOps = operations.VisibleServiceGroups
            .SelectMany(x => x.Children)
            .SelectMany(x => x.Children)
            .Where(x => x.IsSelected);

        var serviceModels = targetedOps
            .GroupBy(x => x.Item.ServiceName)
            .Select(x => new SelectableDynSvcModel(new DynSvc
            {
                ServiceGroupName = x.First().Item.ServiceGroupName,
                Name = x.First().Item.ServiceName,
                Operations = x.Select(y => y.Item).ToArray()
            }, x.ToArray()))
            .ToArray();

        var groupModels = serviceModels
            .GroupBy(x => x.Item.ServiceGroupName)
            .Select(x => new SelectableDynSvcGroupModel(new DynSvcGroup
            {
                Name = x.First().Item.ServiceGroupName,
                Services = x.Select(y => y.Item).ToArray()
            }, x.ToArray()));

        var groups = groupModels.Select(x => x.Item);

        _services = groups.ToArray();
    }

    public async Task SaveToFileAsync()
    {
        using var _ = ILoading.UseLoading(this);

        string content = await _mainService.BuildCustomCollection(new BuildCustomCollectionRequest
        {
            OutputSchema = OutputSchema,
            CollectionName = CollectionName,
            Services = _services,
            Resource = _resource,
            Options = CurrentOptions
        });

        using var stream = new MemoryStream(Encoding.Default.GetBytes(content));

        var fileSaveResult = await _fileSaver.SaveAsync($"{CollectionName}.{GetSuggestedExtension()}", stream);
        OutputPath = fileSaveResult.FilePath ?? string.Empty;
    }

    public async Task ViewFileInFolder()
    {
        await _launcher.OpenAsync(new Uri($"file:///{Path.GetDirectoryName(OutputPath)}"));
    }

    private string GetSuggestedExtension()
    {
        if (OutputSchema == OutputSchemas.CSharp)
        {
            return "cs";
        }

        if (CurrentOptions is SerialisedCollectionBuilderOptions serialisedOptions)
        {
            return serialisedOptions.OutputFormat switch
            {
                OutputFormats.Json => "json",
                OutputFormats.Yaml => "yml",
                _ => "txt"
            };
        }

        return "txt";
    }
}

public interface IBuildCollectionViewModel : ILoading
{
    OutputSchemas[] AvailableOutputSchemas { get; }

    string CollectionName { get; set; }
    OutputSchemas OutputSchema { get; set; }
    ICollectionBuilderOptions CurrentOptions { get; }

    string OutputPath { get; }

    void Init(ICredentialsViewModel credentials, ISelectOperationsViewModel operations);
    Task SaveToFileAsync();
    Task ViewFileInFolder();
}
