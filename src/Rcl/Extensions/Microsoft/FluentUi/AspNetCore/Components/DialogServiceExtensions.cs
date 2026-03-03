using System.Reflection;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Extensions.Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Extensions.Microsoft.FluentUi.AspNetCore.Components;

internal static class DialogServiceExtensions
{
    public static async Task ShowAsPanelAsync<T>(this IDialogService service,
        Dictionary<string, object>? componentParameters = null,
        global::Microsoft.FluentUI.AspNetCore.Components.HorizontalAlignment alignment = 
            global::Microsoft.FluentUI.AspNetCore.Components.HorizontalAlignment.Left
    ) where T : ComponentBase
    {
        componentParameters ??= [];
        var renderFragment = RenderFragmentExtensions.CreateRenderFragment<T>(componentParameters);
        await service.ShowDialogAsync(renderFragment, new DialogParameters
        {
            Title = null,
            TrapFocus = true,
            Modal = true,
            PrimaryAction = null,
            SecondaryAction = null,
            Alignment = alignment,
            DialogType = DialogType.Panel
        });
    }

    public static async Task ShowAsDialogAsync<T>(this IDialogService service,
        Dictionary<string, object>? componentParameters = null) where T : ComponentBase
    {
        componentParameters ??= [];
        var renderFragment = RenderFragmentExtensions.CreateRenderFragment<T>(componentParameters);
        await service.ShowDialogAsync(renderFragment, new DialogParameters
        {
            Title = null,
            TrapFocus = true,
            Modal = true,
            PrimaryAction = null,
            SecondaryAction = null,
        });
    }

    public static Task<IDialogReference> ShowComponentAsync
        (this IDialogService service, ComponentBase component, DialogParameters? dialogParameters = null)
    {
        Type richType = component.GetType();

        var componentParameters = richType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.GetCustomAttribute<ParameterAttribute>() != null)
                .ToDictionary(p => p.Name, p => p.GetValue(component) ?? new object());

        var renderFragment = RenderFragmentExtensions.CreateRenderFragment(richType, componentParameters);
        return service.ShowDialogAsync(renderFragment, dialogParameters ?? new DialogParameters());
    }
}