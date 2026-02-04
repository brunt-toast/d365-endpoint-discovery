using System.Diagnostics;
using System.Reflection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Components.Layout;

public partial class MainLayout
{
#pragma warning disable IL3000
    private static string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion 
                                     ?? string.Empty;
#pragma warning restore IL3000
}