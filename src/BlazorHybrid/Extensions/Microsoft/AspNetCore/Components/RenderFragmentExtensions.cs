using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorHybrid.Extensions.Microsoft.AspNetCore.Components;

internal static class RenderFragmentExtensions
{
    public static RenderFragment CreateRenderFragment<TComponent>(
        IDictionary<string, object>? parameters = null)
        where TComponent : IComponent
    {
        return builder =>
        {
            int seq = 0;
            builder.OpenComponent<TComponent>(seq++);

            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    builder.AddAttribute(seq++, kvp.Key, kvp.Value);
                }
            }

            builder.CloseComponent();
        };
    }

    public static RenderFragment CreateRenderFragment(
        Type componentType,
        IDictionary<string, object>? parameters = null)
    {
        return builder =>
        {
            var seq = 0;
            builder.OpenComponent(seq++, componentType);

            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    builder.AddAttribute(seq++, kvp.Key, kvp.Value);
                }
            }

            builder.CloseComponent();
        };
    }
}
