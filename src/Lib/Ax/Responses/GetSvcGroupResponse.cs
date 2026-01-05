using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Responses;

internal class GetSvcGroupResponse
{
    [JsonProperty("Services")]public DynSvc[] Services { get; set; } = [];
}