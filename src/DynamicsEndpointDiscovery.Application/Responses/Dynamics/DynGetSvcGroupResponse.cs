using DynamicsEndpointDiscovery.Application.Types.Dynamics;
using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Responses.Dynamics;

internal class DynGetSvcGroupResponse
{
    [JsonProperty("Services")]public DynSvc[] Services { get; set; } = [];
}