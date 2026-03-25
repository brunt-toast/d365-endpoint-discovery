using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Components;

public partial class ServiceDiscoveryComponent
{
    private const int IntroStepIndex = 0;
    private const int LastStepIndex = 6;

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

    private async Task GoToPreviousStepAsync()
    {
        if (_wizard.Value > IntroStepIndex)
        {
            await _wizard.GoToStepAsync(_wizard.Value - 1);
        }
    }

    private async Task GoToNextStepAsync()
    {
        if (_wizard.Value < LastStepIndex)
        {
            await _wizard.GoToStepAsync(_wizard.Value + 1, validateEditContexts: true);
        }
    }

    private async Task FinishWizardAsync()
    {
        await _wizard.FinishAsync(validateEditContexts: true);
    }

    private Task RunIfAdvancing(FluentWizardStepChangeEventArgs arg, Func<Task> action)
    {
        return arg.TargetIndex <= _wizard.Value ? Task.CompletedTask : action.Invoke();
    }

    private Task RunIfAdvancing(FluentWizardStepChangeEventArgs arg, Action action)
    {
        if (arg.TargetIndex > _wizard.Value)
        {
            action.Invoke();
        }

        return Task.CompletedTask;
    }
}