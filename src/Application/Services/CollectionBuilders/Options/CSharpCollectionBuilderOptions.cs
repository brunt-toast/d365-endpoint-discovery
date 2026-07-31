using System.ComponentModel.DataAnnotations;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;

public class CSharpCollectionBuilderOptions : ICollectionBuilderOptions
{
    [Display(Name = "Newtonsoft.Json serialisation support")]
    public bool IncludeNewtonsoftJsonAttributes { get; set; } = true;

    [Display(Name = "System.Text.Json serialisation support")]
    public bool IncludeSystemTextJsonAttributes { get; set; } = true;

    public void Validate()
    {
    }

    public bool IsOptionDisabled(string propertyName)
    {
        return false;
    }
}
