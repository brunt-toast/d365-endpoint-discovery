using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorHybrid.Icons;

internal static class FluentIconRepository
{
    public static Icon IntroStepPrevIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.Home();
    public static Icon IntroStepCurIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.Home();
    public static Icon IntroStepNextIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Home();

    public static Icon ConnectStepPrevIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.Password();
    public static Icon ConnectStepCurIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.Password();
    public static Icon ConnectStepNextIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Password();

    public static Icon GroupsStepPrevIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.AppsList();
    public static Icon GroupsStepCurIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.AppsList();
    public static Icon GroupsStepNextIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.AppsList();

    public static Icon ServicesStepPrevIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.ListBarTree();
    public static Icon ServicesStepCurIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.ListBarTree();
    public static Icon ServicesStepNextIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.ListBarTree();

    public static Icon OperationsStepPrevIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.ListBarTreeOffset();
    public static Icon OperationsStepCurIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.ListBarTreeOffset();
    public static Icon OperationsStepNextIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.ListBarTreeOffset();

    public static Icon CollectionStepPrevIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.DocumentSave();
    public static Icon CollectionStepCurIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.DocumentSave();
    public static Icon CollectionStepNextIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.DocumentSave();

    public static Icon FinishStepPrevIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.CheckmarkCircle();
    public static Icon FinishStepCurIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.CheckmarkCircle();
    public static Icon FinishStepNextIcon => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.CheckmarkCircle();
}
