using System.Text;
using BlazorHybrid.Components;
using BlazorHybrid.Extensions.Microsoft.FluentUi.AspNetCore.Components;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using CommunityToolkit.Maui.Storage;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorHybrid.ViewModels;

internal class ServiceDiscoveryViewModel : IServiceDiscoveryViewModel
{
    private readonly IMainService _mainService;
    private readonly IFileSaver _fileSaver;
    private readonly IDialogService _dialogService;
    private readonly SemaphoreSlim _singleOperationSemaphore = new(1, 1);

    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string TokenRequestEndpoint { get; set; } = string.Empty;

    public string GrepGroups { get; set; } = ".*";
    public string GrepServices { get; set; } = ".*";
    public string GrepOperations { get; set; } = ".*";

    public string CollectionName { get; set; } = "Collection";
    public OutputSchemas OutputSchema { get; set; }
    public OutputFormats OutputFormat { get; set; }
    public bool Minify { get; set; }

    public ICollection<OutputSchemas> OutputSchemaOptions { get; } = Enum.GetValues<OutputSchemas>();
    public ICollection<OutputFormats> OutputFormatOptions { get; } = Enum.GetValues<OutputFormats>();

    public bool IsLoading { get; private set; }

    public ServiceDiscoveryViewModel(IMainService mainService, IFileSaver fileSaver, IDialogService dialogService)
    {
        _mainService = mainService;
        _fileSaver = fileSaver;
        _dialogService = dialogService;

        OutputSchema = OutputSchemaOptions.First();
        OutputFormat = OutputFormatOptions.First();
    }

    public async Task DiscoverServices()
    {
        IsLoading = true;
        await _singleOperationSemaphore.WaitAsync();

        try
        {
            string output = await _mainService.GetServiceCollectionAsync(new GetServiceCollectionRequest
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Resource = Resource,
                TokenRequestEndpoint = TokenRequestEndpoint,
                GrepGroupsRegex = GrepGroups,
                GrepServicesRegex = GrepServices,
                GrepOperationsRegex = GrepOperations,
                CollectionName = CollectionName,
                OutputSchema = OutputSchema,
                OutputFormat = OutputFormat,
                Minify = Minify
            });

            using var stream = new MemoryStream(Encoding.Default.GetBytes(output));

            string suggestedExtension = OutputFormat switch
            {
                OutputFormats.Json => "json",
                OutputFormats.Yaml => "yml",
                _ => "txt"
            };

            await _fileSaver.SaveAsync($"{CollectionName}.{suggestedExtension}", stream);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowComponentAsync(new ExceptionComponent
            {
#pragma warning disable BL0005
                Exception = ex,
                Message = "Check configuration and try again."
#pragma warning restore BL0005
            }, new DialogParameters
            {
                Title = "Something went wrong.",
                PrimaryAction = "OK",
            });
        }
        finally
        {
            IsLoading = false;
            _singleOperationSemaphore.Release();
        }
    }
}

internal interface IServiceDiscoveryViewModel
{
    bool IsLoading { get; }

    string ClientId { get; set; }
    string ClientSecret { get; set; }
    string Resource { get; set; }
    string TokenRequestEndpoint { get; set; }

    string GrepGroups { get; set; }
    string GrepServices { get; set; }
    string GrepOperations { get; set; }

    string CollectionName { get; set; }
    OutputSchemas OutputSchema { get; set; }
    OutputFormats OutputFormat { get; set; }
    bool Minify { get; set; }

    ICollection<OutputSchemas> OutputSchemaOptions { get; }
    ICollection<OutputFormats> OutputFormatOptions { get; }

    Task DiscoverServices();
}