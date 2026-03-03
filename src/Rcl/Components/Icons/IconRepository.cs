using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Components.Icons;

internal static class IconRepository
{
    public static Icon Upload { get; } = new Size20.ArrowUpload();
    public static Icon Download { get; } = new Size20.DrawerArrowDownload();
}
