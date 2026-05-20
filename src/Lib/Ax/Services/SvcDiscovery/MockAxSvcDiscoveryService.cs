using System.Text.RegularExpressions;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.SvcDiscovery;

internal class MockAxSvcDiscoveryService : IAxSvcDiscoveryService
{
    private static readonly string[] GroupDomains =
    [
        "Customer", "Supplier", "Inventory", "Warehouse", "Order", "Sales", "Purchase", "Pricing",
        "Tax", "Finance", "Ledger", "Project", "Production", "Quality", "Transport", "Retail",
        "Asset", "Procurement", "Planning", "Analytics", "Compliance", "Identity", "Workflow", "Document"
    ];

    private static readonly string[] GroupCapabilities =
    [
        "Operations", "Management", "Orchestration", "Integration", "Insights", "Services", "Automation",
        "Coordination", "Administration", "Lifecycle", "Hub", "Control", "Platform", "Intelligence"
    ];

    private static readonly string[] ServiceActions =
    [
        "Resolve", "Calculate", "Validate", "Sync", "Schedule", "Assign", "Track", "Create", "Update",
        "Reconcile", "Allocate", "Approve", "Route", "Publish", "Generate", "Archive", "Import", "Export"
    ];

    private static readonly string[] ServiceTargets =
    [
        "Profile", "Account", "Invoice", "Shipment", "Return", "Contract", "Payment", "Quote", "Batch",
        "WorkItem", "Request", "Transaction", "Adjustment", "Forecast", "Template", "Reference", "Order",
        "Agreement", "Document", "Reservation"
    ];

    private static readonly string[] OperationQualifiers =
    [
        "Draft", "Final", "Bulk", "Detailed", "Summary", "Incremental", "Preview", "Validated", "Current",
        "Historic", "Pending", "Approved", "Rejected", "Active", "Archived", "Delta", "Snapshot", "Secure"
    ];

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
        Random random = Random.Shared;
        HashSet<string> usedGroupNames = new(StringComparer.OrdinalIgnoreCase);

        return Enumerable.Range(0, groups)
            .Select(_ =>
            {
                string groupName = NextUniqueName(usedGroupNames,
                    () => $"{Pick(GroupDomains, random)}{Pick(GroupCapabilities, random)}");

                HashSet<string> usedServiceNames = new(StringComparer.OrdinalIgnoreCase);

                return new DynSvcGroup
                {
                    Name = groupName,
                    Services = Enumerable.Range(0, servicesPerGroup)
                        .Select(_ =>
                        {
                            string serviceName = NextUniqueName(usedServiceNames,
                                () => $"{Pick(ServiceActions, random)}{Pick(ServiceTargets, random)}");

                            return new DynSvc
                            {
                                Name = serviceName,
                                ServiceGroupName = groupName,
                                Operations = Enumerable.Range(0, opsPerService)
                                    .Select(_ => new DynSvcOp
                                    {
                                        ServiceGroupName = groupName,
                                        ServiceName = serviceName,
                                        Name =
                                            $"{Pick(ServiceActions, random)}{Pick(OperationQualifiers, random)}{Pick(ServiceTargets, random)}",
                                        Parameters = [],
                                        Return = null
                                    }).ToArray()
                            };
                        }).ToArray()
                };
            });

        static string Pick(IReadOnlyList<string> values, Random random) => values[random.Next(values.Count)];

        static string NextUniqueName(ISet<string> usedNames, Func<string> nameFactory)
        {
            const int maxAttempts = 25;
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                string candidate = nameFactory();
                if (usedNames.Add(candidate))
                {
                    return candidate;
                }
            }

            string fallback = $"{nameFactory()}{Random.Shared.Next(1000, 9999)}";
            usedNames.Add(fallback);
            return fallback;
        }
    }
}