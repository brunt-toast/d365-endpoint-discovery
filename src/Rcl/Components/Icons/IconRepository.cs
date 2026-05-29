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
    public static Icon Remember { get; } = new Size20.Brain();
    public static Icon Certificate { get; } = new Size20.Certificate();
    public static Icon Mock { get; } = new Size20.Wand();
}
