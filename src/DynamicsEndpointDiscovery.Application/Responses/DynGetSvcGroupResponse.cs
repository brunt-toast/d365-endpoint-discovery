using DynamicsEndpointDiscovery.Application.Types;
using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Responses;

internal class DynGetSvcGroupResponse
{
    [JsonProperty("Services")]public DynSvc[] Services { get; set; } = [];
}