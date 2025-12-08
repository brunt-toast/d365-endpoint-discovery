using DynamicsEndpointDiscovery.Application.Mapping;
using DynamicsEndpointDiscovery.Application.Types.Dynamics;
using DynamicsEndpointDiscovery.Application.Types.OpenApi;

namespace DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public class OpenApiCollectionBuilder  : CollectionBuilderBase<OpenApiCollection>
{
    protected override OpenApiCollection BuildTypedCollection(IEnumerable<DynSvcGroup> groups, string resource, string collectionName = "Collection")
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
            Paths = GetPathDefns(groupsList).ToDictionary(),
            Components = new OpenApiComponentDefn
            {
                Schemas = GetSchemaDefns(groupsList).ToDictionary()
            }
        };
    }

    private static IEnumerable<KeyValuePair<string, OpenApiSchemaDefn>> GetSchemaDefns(IEnumerable<DynSvcGroup> groups)
    {
        var operations = groups.SelectMany(x => x.Services).SelectMany(x => x.Operations);
        foreach (var operation in operations)
        {
            OpenApiSchemaDefn sd = new OpenApiSchemaDefn
            {
                Parameters = [new OpenApiParameterDefn
                    {
                        Type = "object",
                        Required = [],
                        Properties = operation.Parameters.Select(parameter => new KeyValuePair<string, OpenApiTypeDefn>(parameter.Name, new OpenApiTypeDefn
                        {
                            Type = DynamicsToJsonTypeMapper.MapType(parameter.Type)
                        })).ToDictionary()
                    }
                ]
            };

            yield return new KeyValuePair<string, OpenApiSchemaDefn>(
                $"{operation.ServiceGroupName}_{operation.ServiceName}_{operation.Name}", sd);
        }
    }

    private static IEnumerable<KeyValuePair<string, OpenApiPathDefn>> GetPathDefns(IEnumerable<DynSvcGroup> groups)
    {
        var operations = groups.SelectMany(x => x.Services).SelectMany(x => x.Operations);
        foreach (var operation in operations)
        {
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
                        ContentTypesToSchemaRefs = new Dictionary<string, OpenApiSchema>
                        {
                            {"application/json", new OpenApiSchema
                                {
                                    Schema = new OpenApiSchemaRef
                                    {
                                        Ref = $"#/components/schemas/{operation.ServiceGroupName}_{operation.ServiceName}_{operation.Name}"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            yield return new KeyValuePair<string, OpenApiPathDefn>(
                $"/api/services/{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}", pd);
        }
    }
}
