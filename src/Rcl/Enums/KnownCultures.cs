using System.ComponentModel.DataAnnotations;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;

public enum KnownCultures
{
    [Display(Name="English")]
    En,

    [Display(Name= "Fran\u00e7ais")]
    Fr,

    [Display(Name="\u65e5\u672c\u8a9e")]
    Ja,

    [Display(Name = "Esperanto")]
    Eo,

    [Display(Name = "Linguam latinam")]
    La,

    [Display(Name = "Español")]
    Es,
}
