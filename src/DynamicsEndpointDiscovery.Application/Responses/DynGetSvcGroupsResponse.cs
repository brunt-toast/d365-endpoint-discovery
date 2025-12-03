using DynamicsEndpointDiscovery.Application.Types;
using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Responses;

internal class DynGetSvcGroupsResponse
{
    [JsonProperty("ServiceGroups")] public DynSvcGroup[] Groups { get; set; } = [];
}