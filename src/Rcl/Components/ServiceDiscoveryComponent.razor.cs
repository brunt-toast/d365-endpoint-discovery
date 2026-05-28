using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Components;

public partial class ServiceDiscoveryComponent
{
    private const int IntroStepIndex = 0;
    private const int LastStepIndex = 7;

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

    protected override void OnInitialized()
    {
        ConnectionOptionsVm.Init();
        base.OnInitialized();
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

    private async Task RunIfAdvancing(FluentWizardStepChangeEventArgs arg, Func<Task> action, bool runWithAwait = false)
    {
        if (arg.TargetIndex > _wizard.Value)
        {
            if (runWithAwait)
            {
                await action.Invoke();
            }
            else
            {

                _ = RunInBackgroundAndRefreshAsync(action);
            }
        }
    }

    private async Task RunInBackgroundAndRefreshAsync(Func<Task> action)
    {
        try
        {
            await action.Invoke();
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
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