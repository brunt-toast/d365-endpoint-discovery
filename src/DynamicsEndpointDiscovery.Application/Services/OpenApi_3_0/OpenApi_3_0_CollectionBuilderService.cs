using DynamicsEndpointDiscovery.Application.Types;
using DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

namespace DynamicsEndpointDiscovery.Application.Services.OpenApi_3_0;

public class OpenApi_3_0_CollectionBuilderService
{
    public OpenApiCollection BuildOpenApiCollection(IEnumerable<DynSvcGroup> groups, string resource)
    {
        var groupsList = groups.ToList();

        var info = new OpenApiInfo
        {
            Version = "3.0.0",
            Title = "Collection",
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

    private IEnumerable<KeyValuePair<string, OpenApiSchemaDefn>> GetSchemaDefns(IEnumerable<DynSvcGroup> groups)
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
                            Type = "string"
                        })).ToDictionary()
                    }
                ]
            };

            yield return new KeyValuePair<string, OpenApiSchemaDefn>(
                $"{operation.ServiceGroupName}_{operation.ServiceName}_{operation.Name}", sd);
        }
    }

    private IEnumerable<KeyValuePair<string, OpenApiPathDefn>> GetPathDefns(IEnumerable<DynSvcGroup> groups)
    {
        var operations = groups.SelectMany(x => x.Services).SelectMany(x => x.Operations);
        foreach (var operation in operations)
        {
            OpenApiPathDefn pd = new OpenApiPathDefn
            {
                Post = new OpenApiPostRequestDefn
                {
                    Description = $"{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}",
                    OperationId = $"{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}",
                    RequestBody = new OpenApiRequestBodyDefn
                    {
                        Description = $"{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}",
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
                $"{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}", pd);
        }
    }
}
