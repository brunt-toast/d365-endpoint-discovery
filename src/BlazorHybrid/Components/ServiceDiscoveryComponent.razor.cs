using Microsoft.FluentUI.AspNetCore.Components;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Components;

public partial class ServiceDiscoveryComponent
{
    private FluentWizard _wizard = null!;

    private async Task OnWizardFinish()
    {
        CredentialsVm.ClearValues();
        await _wizard.GoToStepAsync(0);
    }
}