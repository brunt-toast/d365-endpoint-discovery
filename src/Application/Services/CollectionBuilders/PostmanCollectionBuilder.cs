using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Mapping;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.Postman;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public class PostmanCollectionBuilder : CollectionBuilderBase<PostmanCollection>
{
    private readonly ILogger _logger;

    public PostmanCollectionBuilder(ILogger logger)
    {
        _logger = logger;
    }

    protected override PostmanCollection BuildTypedCollection(IEnumerable<DynSvcGroup> groups,
        Dictionary<string, string> typeDefs, 
        string resource, 
        string collectionName = "Collection")
    {
        var collectionInfo = new PostmanCollectionInfo
        {
            PostmanId = Guid.CreateVersion7().ToString(),
            Name = collectionName,
            Schema = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
            ExporterId = ""
        };

        return new PostmanCollection
        {
            Info = collectionInfo,
            Items = groups.Select(x => GetPostmanItem(x, typeDefs)).ToArray()
        };
    }

    private PostmanItem GetPostmanItem(DynSvcGroup group, Dictionary<string, string> typeDefs)
    {
        return new PostmanItem
        {
            Name = group.Name,
            Items = group.Services.Select(x => GetPostmanItem(x, typeDefs)).ToArray(),
            Request = null,
            Response = null
        };
    }

    private PostmanItem GetPostmanItem(DynSvc service, Dictionary<string, string> typeDefs)
    {
        return new PostmanItem
        {
            Name = service.Name,
            Items = service.Operations.Select(x => GetPostmanItem(x, typeDefs)).ToArray(),
            Request = null,
            Response = null
        };
    }

    private PostmanItem GetPostmanItem(DynSvcOp operation, Dictionary<string, string> typeDefs)
    {
        Dictionary<string, object> p = [];
        foreach (var param in operation.Parameters)
        {
            if (typeDefs.TryGetValue(param.Type, out string? typeDef))
            {
                p[param.Name] = JObject.Parse(typeDef);
            }
            else
            {
                _logger.LogWarning("We don't have a definition for type {paramType} for parameter {paramName} " +
                                   "of {operationServiceGroupName}/{operationServiceName}/{operationName}",
                    param.Type, param.Name, operation.ServiceGroupName, operation.ServiceName, operation.Name);
                p[param.Name] = $"[Unknown type {param.Type}]";
            }
        }

        PostmanBody body = new()
        {
            Mode = "raw",
            Raw = JsonConvert.SerializeObject(p, Formatting.Indented)
        };

        PostmanUrl uri = new PostmanUrl
        {
            Raw = $"{{{{resource}}}}/api/services/{operation.ServiceGroupName}/{operation.ServiceName}/{operation.Name}",
            Host = ["{{resource}}"],
            Path = ["api", "services", operation.ServiceGroupName, operation.ServiceName, operation.Name]
        };

        PostmanRequest request = new()
        {
            Method = "POST",
            Headers = [new PostmanHeader
                {
                    Key = "Authorization",
                    Value = "Bearer {{bearerToken}}",
                    Type = "text"
                }
            ],
            Body = body,
            Url = uri
        };

        return new PostmanItem
        {
            Name = operation.Name,
            Request = request,
            Response = [],
            Items = null
        };
    }
}
