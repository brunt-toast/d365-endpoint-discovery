using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Responses;

internal class GetSvcGroupsResponse
{
    [JsonProperty("ServiceGroups")] public DynSvcGroup[] Groups { get; set; } = [];
}