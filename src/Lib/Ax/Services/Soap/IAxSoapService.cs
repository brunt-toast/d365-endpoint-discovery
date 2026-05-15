namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

public interface IAxSoapService
{
    Task<Dictionary<string, string>> GetDataContractsForServices(IEnumerable<string> serviceNames);
}