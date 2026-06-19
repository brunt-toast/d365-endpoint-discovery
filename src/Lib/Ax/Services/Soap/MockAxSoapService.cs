using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal class MockAxSoapService : IAxSoapService
{
    public Task<SoapTypeCollection> GetDataContractsForServices(IEnumerable<string> serviceNames)
    {
        return Task.FromResult(new SoapTypeCollection
        {
            Samples = serviceNames.Select(x => new KeyValuePair<string, string>(x, string.Empty)).ToDictionary(),
            Definitions = []
        });
    }
}
