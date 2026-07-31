using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Rcl.Tests.DataSources.Tests.Services;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Rcl.Tests.Tests.Services;

[TestClass]
[DoNotParallelize]
public class CultureServiceTests
{
    [TestMethod]
    [KnownCultureDataSource]
    public void KnownCultures_ShouldResolve(KnownCultures culture)
    {
        var sc = new ServiceCollection();
        RclServiceRegistrar.RegisterServices(sc);
        IServiceProvider sp = sc.BuildServiceProvider();
        var sut = sp.GetRequiredService<ICultureService>();
        sut.SetCulture(culture);
    }

}
