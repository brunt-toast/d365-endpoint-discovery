using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Services;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Rcl.Tests.Tests.Services;

[TestClass]
[DoNotParallelize]
public class CultureServiceTests
{
    public static IEnumerable<object[]> GetKnownCultures()
    {
        return Enum.GetValues<KnownCultures>().Select(value => (object[])[value]);
    }

    [TestMethod]
    [DynamicData(nameof(GetKnownCultures))]
    public void KnownCultures_ShouldResolve(KnownCultures culture)
    {
        var sut = new CultureService();
        sut.SetCulture(culture);
    }
}
