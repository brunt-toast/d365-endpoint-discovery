using System;
using System.Collections.Generic;
using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorHybrid.Models;

internal class ServiceGroupTreeViewItem : ITreeViewItem
{
    public string Id { get; set; }
    public string Text { get; set; }
    public IEnumerable<ITreeViewItem>? Items { get; set; }
    public Icon? IconCollapsed { get; set; }
    public Icon? IconExpanded { get; set; }
    public bool Disabled { get; set; }
    public bool Expanded { get; set; }
    public Func<TreeViewItemExpandedEventArgs, Task>? OnExpandedAsync { get; set; }

    public ServiceGroupTreeViewItem(DynSvcGroup group)
    {
        Id = group.Name;
        Text = group.Name;
    }
}
