using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public class OpenApiCollectionBuilder : SerialisedCollectionBuilderBase<OpenApiCollectionBuilderOptions>
{
    public OpenApiCollectionBuilder(SerialiserFactory serialiserFactory) : base(serialiserFactory)
    {
    }

    protected override object BuildSerializableCollection(
        IEnumerable<DynSvcGroup> groups,
        SoapTypeCollection types,
        string resource,
        string collectionName,
        OpenApiCollectionBuilderOptions options)
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
                new OpenApiServerDefn { Uri = resource }
            ],
            Paths = GetPathDefns(groupsList, types.Samples).ToDictionary()
        };
    }

    private static IEnumerable<KeyValuePair<string, OpenApiPathDefn>> GetPathDefns(
        IEnumerable<DynSvcGroup> groups,
        Dictionary<string, string> typeDefs)
    {
        var operations = groups.SelectMany(x => x.Services).SelectMany(x => x.Operations);
        foreach (var operation in operations)
        {
            var resolvedBody = operation.Parameters
                .Select(x => new KeyValuePair<string, JObject>(
                    x.Name,
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

            JObject responseBodyContent = JObject.Parse("""
                                                          {
                                                           "application/json": {
                                                             "schema": {
                                                               "type": "object"
                                                             },
                                                             "example": {}
                                                           }
                                                         }
                                                       """);
            responseBodyContent["application/json"]!["example"] =
                JObject.Parse(typeDefs.FirstOrDefault(x => x.Key == operation.Return?.Type).Value ??
                              $"{{\"Unknown type\": \"{operation.Return?.Type}\"}}");

            OpenApiPathDefn pd = new()
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
                    },
                    Responses = new Dictionary<int, OpenApiResponseDefn>
                    {
                        {
                            200, new OpenApiResponseDefn
                            {
                                Description = operation.Return?.Name ?? string.Empty,
                                Content = responseBodyContent
                            }
                        }
                    }
                }
            };

            yield return new KeyValuePair<string, OpenApiPathDefn>(
                $"/api/services/{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}",
                pd);
        }
    }
}
