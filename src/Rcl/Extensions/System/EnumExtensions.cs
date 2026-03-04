using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Extensions.System;

internal static class EnumExtensions
{
    public static string ToDisplayName(this Enum source)
    {
        string sourceString = source.ToString();
        return source.GetType()
            .GetMember(sourceString)
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName() ?? sourceString;
    }

    public static string KnownCulturesToDisplayName(KnownCultures source) => ToDisplayName(source);
}
