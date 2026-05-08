using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Services;

internal class CultureService : ICultureService
{
    private readonly IServiceProvider _services;

    public CultureService(IServiceProvider services)
    {
        _services = services;
    }

    public void SetCulture(KnownCultures culture)
    {
        string cultureKey = culture switch
        {
            KnownCultures.En => "en-GB",
            KnownCultures.Fr => "fr-FR",
            KnownCultures.Ja => "ja-JA",
            KnownCultures.Eo => "eo-EO",
            KnownCultures.La => "la-LA",
            KnownCultures.Es => "es-ES",
            _ => throw new ArgumentOutOfRangeException(nameof(culture), culture, null)
        };

        var cultureInfo = new CultureInfo(cultureKey);
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }

    public string LocaliseEnum<TEnum,TResources>(TEnum value) where TEnum : struct, Enum
    {
        string? localised = _services.GetService<IStringLocalizer<TResources>>()?[value.ToString()].Value;
        return string.IsNullOrWhiteSpace(localised) ? value.ToString() : localised;
    }
}

internal interface ICultureService
{
    void SetCulture(KnownCultures culture);
    string LocaliseEnum<TEnum, TResources>(TEnum value) where TEnum : struct, Enum;
}