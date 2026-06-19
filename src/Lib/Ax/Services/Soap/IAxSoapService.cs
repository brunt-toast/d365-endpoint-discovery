using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

public interface IAxSoapService
{
    Task<SoapTypeCollection> GetDataContractsForServices(IEnumerable<string> serviceNames);
}
