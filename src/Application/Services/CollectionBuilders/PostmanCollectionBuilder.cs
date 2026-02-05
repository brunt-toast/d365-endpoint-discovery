using System.Diagnostics.CodeAnalysis;
using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Mapping;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.Postman;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Xpp;
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
            else if (ResolvePrimitive(param.Type, out object? typeDef2))
            {
                p[param.Name] = typeDef2;
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
            Raw = JsonConvert.SerializeObject(p, Formatting.Indented),
            Options = new PostmanBodyOptions
            {
                Raw = new RawPostmanBodyOptions
                {
                    Language = "json"
                }
            }
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
            Headers =
            [
                new PostmanHeader
                {
                    Key = "Authorization",
                    Value = "Bearer {{bearerToken}}",
                    Type = "text"
                }
            ],
            Body = body,
            Url = uri,
            Description = BuildDescription(operation, typeDefs)
        };

        return new PostmanItem
        {
            Name = operation.Name,
            Request = request,
            Response = [],
            Items = null
        };
    }

    private static string BuildDescription(DynSvcOp operation, Dictionary<string, string> typeDefs)
    {
        StringBuilder dsb = new();
        dsb.AppendLine($"# {operation.Name}");
        dsb.AppendLine($"Service operation in {operation.ServiceGroupName}/{operation.ServiceName}");
        dsb.AppendLine("<hr />");

        dsb.AppendLine();
        dsb.AppendLine("## Known Request Types");
        var paramTypeNames = operation.Parameters.Select(x => x.Type);
        var knownTypes = typeDefs.Where(x => paramTypeNames.Contains(x.Key)).ToList();
        if (knownTypes.Count > 0)
        {
            foreach (var t in knownTypes)
            {
                dsb.AppendLine($"<b>{t.Key}</b>");
                dsb.AppendLine("```json");
                dsb.AppendLine(t.Value);
                dsb.AppendLine("```");
            }
        }
        else
        {
            dsb.AppendLine("There are no known types for this request.");
        }

        dsb.AppendLine();
        dsb.AppendLine("## Unknown Request Types");
        var unknownTypes = paramTypeNames.Where(x => !typeDefs.Keys.Contains(x)).ToList();
        if (unknownTypes.Count > 0)
        {
            dsb.AppendLine("<ul>");
            foreach (var t in unknownTypes)
            {
                dsb.AppendLine($"<li>{t}</li>");
            }
            dsb.AppendLine("</ul>");
        }
        else
        {
            dsb.AppendLine("There are no unknown types for this request.");
        }

        dsb.AppendLine();
        dsb.AppendLine("## Return Type");
        if (operation.Return is not null)
        {
            var returnTypeDefn = typeDefs.GetValueOrDefault(operation.Return.Type);
            if (returnTypeDefn is not null)
            {
                dsb.AppendLine($"<b>{operation.Return.Type}</b>");
                dsb.AppendLine("```json");
                dsb.AppendLine(returnTypeDefn);
                dsb.AppendLine("```");
            }
            else
            {
                dsb.AppendLine($"{operation.Return.Type} (unknown definition)");
            }
        }
        else
        {
            dsb.AppendLine("This operation doesn't return anything.");
        }

        return dsb.ToString();
    }

    private static bool ResolvePrimitive(string key, [NotNullWhen(true)] out object? defaultValue)
    {
        if (key.EndsWith("[]"))
        {
            defaultValue = new[] { $"[{key}]" };
        }
        else
        {
            defaultValue = key switch
            {
                "String" => string.Empty,
                "List`1" => Array.Empty<object>(),
                "Boolean" => false,
                "Int32" => int.MaxValue,
                "Int64" => long.MaxValue,
                "DateTime" => (DateTime)XppDateTime.MaxValue,
                "Guid" => Guid.AllBitsSet,
                "Double" => double.MaxValue,
                "Decimal" => decimal.MaxValue,
                "Float" => float.MaxValue,
                _ => null
            };
        }

        return defaultValue is not null;
    }
}
