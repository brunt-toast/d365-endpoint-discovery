using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public class OpenApiCollectionBuilder : CollectionBuilderBase<OpenApiCollection>
{
    protected override OpenApiCollection BuildTypedCollection(IEnumerable<DynSvcGroup> groups,
        Dictionary<string, string> typeDefs,
        string resource,
        string collectionName = "Collection")
    {
        var groupsList = groups.ToList();

        var info = new OpenApiInfo
        {
            Version = "3.0.0",
            Title = collectionName,
            Description = "Dynamics 365 service endpoints",
            TermsOfService = "http://swagger.io/terms/",
            Contact = new OpenApiContactInfo
            {
                Name = string.Empty,
                Email = string.Empty,
                Url = string.Empty
            },
            License = new OpenApiLicenseInfo
            {
                Name = "All rights reserved",
                Url = string.Empty
            }
        };

        return new OpenApiCollection
        {
            Info = info,
            Servers =
            [
                new OpenApiServerDefn {Uri = resource}
            ],
            Paths = GetPathDefns(groupsList, typeDefs).ToDictionary()
        };
    }

    private static IEnumerable<KeyValuePair<string, OpenApiPathDefn>> GetPathDefns(IEnumerable<DynSvcGroup> groups,
        Dictionary<string, string> typeDefs)
    {
        var operations = groups.SelectMany(x => x.Services).SelectMany(x => x.Operations);
        foreach (var operation in operations)
        {
            var resolvedBody = operation.Parameters
                .Select(x => new KeyValuePair<string, JObject>(x.Name, 
                    JObject.Parse(typeDefs.FirstOrDefault(y => y.Key == x.Type).Value ?? $"{{\"Unknown Type\": \"{x.Type}\"}}")))
                .ToDictionary();

            JObject requestBodyContent = JObject.Parse("""
                                                          {
                                                           "application/json": {
                                                             "schema": {
                                                               "type": "object"
                                                             },
                                                             "example": {}
                                                           }
                                                         }
                                                       """);
            requestBodyContent["application/json"]!["example"] = JObject.Parse(JsonConvert.SerializeObject(resolvedBody));

            OpenApiPathDefn pd = new OpenApiPathDefn
            {
                Post = new OpenApiPostRequestDefn
                {
                    Description = $"/api/services/{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}",
                    OperationId = $"/api/services/{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}",
                    RequestBody = new OpenApiRequestBodyDefn
                    {
                        Description = $"/api/services/{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}",
                        IsRequired = false,
                        Content = requestBodyContent
                    }
                }
            };

            yield return new KeyValuePair<string, OpenApiPathDefn>(
                        $"/api/services/{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}", pd);
        }
    }
}
