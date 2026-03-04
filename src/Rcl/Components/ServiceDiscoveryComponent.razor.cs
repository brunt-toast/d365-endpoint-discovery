using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Components;

public partial class ServiceDiscoveryComponent
{
    private FluentWizard _wizard = null!;

    private KnownCultures Culture
    {
        get;
        set
        {
            field = value;
            CultureService.SetCulture(value);
        }
    }

    private async Task OnWizardFinish()
    {
        CredentialsVm.ClearValues();
        await _wizard.GoToStepAsync(0);
    }
}