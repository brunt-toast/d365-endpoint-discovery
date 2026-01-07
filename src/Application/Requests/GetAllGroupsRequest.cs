using System;
using System.Collections.Generic;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;

public class GetAllGroupsRequest : IHasAxCredentials
{
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string Resource { get; init; }
    public required string TokenRequestEndpoint { get; init; }
}