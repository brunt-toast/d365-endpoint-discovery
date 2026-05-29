namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal class MockAxSoapService : IAxSoapService
{
    public Task<Dictionary<string, string>> GetDataContractsForServices(IEnumerable<string> serviceNames)
    {
        return Task.FromResult(serviceNames.Select(x => new KeyValuePair<string,string>(x, string.Empty)).ToDictionary());
    }
}
