using DynamicsEndpointDiscovery.Application.Types.Dynamics;
using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Responses.Dynamics;

internal class DynGetSvcGroupsResponse
{
    [JsonProperty("ServiceGroups")] public DynSvcGroup[] Groups { get; set; } = [];
}