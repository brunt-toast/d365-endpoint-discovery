using System.ComponentModel.DataAnnotations;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;

public enum OutputSchemas
{
    Default,
    Postman,
    OpenApi,
    [Display(Name = "C#")]
    CSharp
}
