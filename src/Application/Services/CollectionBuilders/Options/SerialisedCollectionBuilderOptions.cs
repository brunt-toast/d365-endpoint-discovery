using System.ComponentModel.DataAnnotations;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;

public abstract class SerialisedCollectionBuilderOptions : ICollectionBuilderOptions
{
    [Display(Name = "Format")]
    public OutputFormats OutputFormat { get; set; } = OutputFormats.Json;

    [Display(Name = "Minify")]
    public bool Minify { get; set; } = true;

    public void Validate()
    {
        if (OutputFormat == OutputFormats.Yaml)
        {
            Minify = false;
        }
    }

    public bool IsOptionDisabled(string propertyName)
    {
        return propertyName == nameof(Minify) && OutputFormat == OutputFormats.Yaml;
    }
}
