using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;
using System.Globalization;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Services;

internal class CultureService : ICultureService
{
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
}

internal interface ICultureService
{
    void SetCulture(KnownCultures culture);
}