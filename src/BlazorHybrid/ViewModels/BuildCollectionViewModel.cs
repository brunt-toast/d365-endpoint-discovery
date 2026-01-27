using System.Text;
using CommunityToolkit.Maui.Storage;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Models;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.ViewModels;

internal class BuildCollectionViewModel : IBuildCollectionViewModel
{
    private readonly IMainService _mainService;
    private readonly IFileSaver _fileSaver;
    private readonly ILauncher _launcher;

    private string _resource = string.Empty;
    private DynSvcGroup[] _services = [];

    public OutputSchemas[] AvailableOutputSchemas { get; } = Enum.GetValues<OutputSchemas>();
    public OutputFormats[] AvailableOutputFormats { get; } = Enum.GetValues<OutputFormats>();

    public string CollectionName { get; set; } = "Collection";
    public OutputSchemas OutputSchema { get; set; }
    public OutputFormats OutputFormat { get; set; }
    public bool Minify { get; set; } = true;

    public string OutputPath { get; private set; } = string.Empty;

    public BuildCollectionViewModel(IMainService mainService, IFileSaver fileSaver, ILauncher launcher)
    {
        _mainService = mainService;
        _fileSaver = fileSaver;
        _launcher = launcher;

        OutputSchema = AvailableOutputSchemas.First(x => x == OutputSchemas.Postman);
        OutputFormat = AvailableOutputFormats.First(x => x == OutputFormats.Json);
    }

    public void Init(ICredentialsViewModel credentials, ISelectOperationsViewModel operations)
    {
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
            }, x.ToArray())).ToArray();

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
        string content = _mainService.BuildCustomCollection(new BuildCustomCollectionRequest
        {
            OutputSchema = OutputSchema,
            OutputFormat = OutputFormat,
            CollectionName = CollectionName,
            Services = _services,
            Resource = _resource,
            Minify = Minify
        });

        using var stream = new MemoryStream(Encoding.Default.GetBytes(content));

        string suggestedExtension = OutputFormat switch
        {
            OutputFormats.Json => "json",
            OutputFormats.Yaml => "yml",
            _ => "txt"
        };

        var fileSaveResult = await _fileSaver.SaveAsync($"{CollectionName}.{suggestedExtension}", stream);
        OutputPath = fileSaveResult.FilePath ?? string.Empty;
    }

    public async Task ViewFileInFolder()
    {
        await _launcher.OpenAsync(new Uri($"file:///{Path.GetDirectoryName(OutputPath)}"));
    }
}

public interface IBuildCollectionViewModel
{
    OutputSchemas[] AvailableOutputSchemas { get; }
    OutputFormats[] AvailableOutputFormats { get; }

    string CollectionName { get; set; }
    OutputSchemas OutputSchema { get; set; }
    OutputFormats OutputFormat { get; set; }
    bool Minify { get; set; }

    string OutputPath { get; }

    void Init(ICredentialsViewModel credentials, ISelectOperationsViewModel operations);
    Task SaveToFileAsync();
    Task ViewFileInFolder();
}