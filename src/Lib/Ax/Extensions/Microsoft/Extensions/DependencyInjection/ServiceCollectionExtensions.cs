using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Extensions.Microsoft.Extensions.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static void AddMockable<TService, TMock, TReal>(
        this IServiceCollection serviceCollection,
        ServiceLifetime serviceLifetime,
        Func<IServiceProvider, bool> mockPredicate)
        where TMock : notnull
        where TReal : notnull
    {
        const string key = "__MockableDi_AddMockable_DoNotUseExternally";

        serviceCollection.Add(new ServiceDescriptor(typeof(TMock), key, typeof(TMock), serviceLifetime));
        serviceCollection.Add(new ServiceDescriptor(typeof(TReal), key, typeof(TReal), serviceLifetime));

        var serviceDescriptor = new ServiceDescriptor(typeof(TService), Factory, serviceLifetime);
        serviceCollection.Add(serviceDescriptor);
        return;

        object Factory(IServiceProvider sp) =>
            mockPredicate.Invoke(sp)
                ? sp.GetRequiredKeyedService<TMock>(key)
                : sp.GetRequiredKeyedService<TReal>(key);
    }
}
