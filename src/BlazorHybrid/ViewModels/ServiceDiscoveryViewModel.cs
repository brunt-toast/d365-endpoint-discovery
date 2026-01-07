using BlazorHybrid.Components;
using BlazorHybrid.Extensions.Microsoft.FluentUi.AspNetCore.Components;
using CommunityToolkit.Maui.Storage;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

    public string GrepGroups { get; set; } = string.Empty;
    public string GrepServices { get; set; } = string.Empty;
    public string GrepOperations { get; set; } = string.Empty;

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
            if (!await ValidateInput())
            {
                return;
            }

            string output = await _mainService.GetServiceCollectionAsync(new GetServiceCollectionRequest
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Resource = Resource,
                TokenRequestEndpoint = TokenRequestEndpoint,
                GrepGroupsRegex = string.IsNullOrWhiteSpace(GrepGroups) ? ".*" : GrepGroups,
                GrepServicesRegex = string.IsNullOrWhiteSpace(GrepServices) ? ".*" : GrepServices,
                GrepOperationsRegex = string.IsNullOrWhiteSpace(GrepOperations) ? ".*" : GrepOperations,
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

    private async Task<bool> ValidateInput()
    {
        List<string> errors = [];

        if (!Uri.TryCreate(TokenRequestEndpoint, UriKind.Absolute, out _))
        {
            errors.Add("Token request endpoint must be a valid absolute URI");
        }

        if (!Uri.TryCreate(Resource, UriKind.Absolute, out _))
        {
            errors.Add("Resource must be a valid absolute URI");
        }

        if (!Guid.TryParse(ClientId, out _))
        {
            errors.Add("Client ID must be a valid GUID");
        }

        if (!string.IsNullOrWhiteSpace(GrepGroups))
        {
            try
            {
                _ = Regex.Match(string.Empty, GrepGroups);
            }
            catch (ArgumentException)
            {
                errors.Add($"\"{GrepGroups}\" (groups) is not a valid regular expression.");
            }
        }

        if (!string.IsNullOrWhiteSpace(GrepOperations))
        {
            try
            {
                _ = Regex.Match(string.Empty, GrepOperations);
            }
            catch (ArgumentException)
            {
                errors.Add($"\"{GrepOperations}\" (operations) is not a valid regular expression.");
            }
        }

        if (!string.IsNullOrWhiteSpace(GrepServices))
        {
            try
            {
                _ = Regex.Match(string.Empty, GrepServices);
            }
            catch (ArgumentException)
            {
                errors.Add($"\"{GrepServices}\" (services) is not a valid regular expression.");
            }
        }

        if (errors.Count > 0)
        {
            await _dialogService.ShowComponentAsync(new TextComponent
            {
#pragma warning disable BL0005
                Text = new MarkupString(string.Join(string.Empty, errors.Select(x => $"- {x}<br />")))
#pragma warning restore BL0005
            },
                new DialogParameters
                {
                    Title = "Invalid request"
                });
            return false;
        }

        return true;
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