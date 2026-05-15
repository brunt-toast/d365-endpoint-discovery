using System.Text.RegularExpressions;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.SvcDiscovery;

internal class MockAxSvcDiscoveryService : IAxSvcDiscoveryService
{
    public async Task<IEnumerable<DynSvcGroup>> MapServicesAsync(string grepGroupsRegexString = ".*",
        string grepServicesRegexString = ".*",
        string grepOperationsRegexString = ".*")
    {
        Regex grepGroupsRegex = new(grepGroupsRegexString);
        Regex grepServicesRegex = new(grepServicesRegexString);
        Regex grepOperationsRegex = new(grepOperationsRegexString);

        var groups = (await GetAllGroups())
            .Where(x => grepGroupsRegex.IsMatch(x.Name))
            .ToList();

        foreach (var service in groups.SelectMany(x => x.Services))
        {
            service.Operations = service.Operations.Where(x => grepOperationsRegex.IsMatch(x.Name)).ToArray();
        }

        foreach (var group in groups)
        {
            group.Services = group.Services.Where(x => grepServicesRegex.IsMatch(x.Name)).ToArray();
        }

        return groups;
    }

    public async Task<IEnumerable<DynSvcGroup>> GetAllGroups()
    {
        await Task.Yield();

        var groups = CreateGroups(20, 10, 5).ToList();
        foreach (var group in groups)
        {
            group.Services = [];
        }

        return groups.AsEnumerable();
    }

    public async Task<IEnumerable<DynSvc>> GetServicesForGroups(IEnumerable<DynSvcGroup> groups)
    {
        await Task.Yield();

        var services = CreateGroups(20, 10, 5).SelectMany(x => x.Services).ToList();
        foreach (var service in services)
        {
            service.Operations = [];
        }

        return services.AsEnumerable();
    }

    public async Task<IEnumerable<DynSvcOp>> GetOperationsForServices(IEnumerable<DynSvc> services)
    {
        await Task.Yield();

        var groups = CreateGroups(20, 10, 5).SelectMany(x => x.Services).SelectMany(x => x.Operations);
        return groups;
    }

    private static IEnumerable<DynSvcGroup> CreateGroups(int groups, int servicesPerGroup, int opsPerService)
    {
        return Enumerable.Range(0, groups)
            .Select(i => new DynSvcGroup
            {
                Name = $"ServiceGroup{CharFor(i)}",
                Services = Enumerable.Range(0, servicesPerGroup)
                    .Select(j => new DynSvc
                    {
                        Name = $"Service{CharFor(i)}_{CharFor(j)}",
                        ServiceGroupName = $"ServiceGroup{CharFor(i)}",
                        Operations = Enumerable.Range(0, opsPerService)
                            .Select(k => new DynSvcOp
                            {
                                ServiceGroupName = $"ServiceGroup{CharFor(i)}",
                                ServiceName = $"Service{CharFor(i)}_{CharFor(j)}",
                                Name = $"Service{CharFor(i)}_{CharFor(j)}_{CharFor(k)}",
                                Parameters = [],
                                Return = null
                            }).ToArray()
                    }).ToArray()
            });

        static string CharFor(int i) => ((char)(i + 65)).ToString();
    }
}