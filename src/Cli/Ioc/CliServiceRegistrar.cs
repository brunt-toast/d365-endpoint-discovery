using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;

public static class CliServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        ApplicationServiceRegistrar.RegisterServices(sc);
        sc.AddTransient<DynSvcDiscoveryRootCommand>();
    }
}
