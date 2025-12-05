using System.Text;
using DynamicsEndpointDiscovery.Application.Types;
using DynamicsEndpointDiscovery.Application.Types.Dynamics;
using DynamicsEndpointDiscovery.Application.Types.Postman;

namespace DynamicsEndpointDiscovery.Application.Services.Postman;

public class PostmanCollectionBuilderService
{
    public PostmanCollection BuildPostmanCollection(IEnumerable<DynSvcGroup> groups, string collectionName = "Collection")
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
            Items = groups.Select(GetPostmanItem).ToArray()
        };
    }

    private PostmanItem GetPostmanItem(DynSvcGroup group)
    {
        return new PostmanItem
        {
            Name = group.Name,
            Items = group.Services.Select(GetPostmanItem).ToArray(),
            Request = null,
            Response = null
        };
    }

    private PostmanItem GetPostmanItem(DynSvc service)
    {
        return new PostmanItem
        {
            Name = service.Name,
            Items = service.Operations.Select(GetPostmanItem).ToArray(),
            Request = null,
            Response = null
        };
    }

    private PostmanItem GetPostmanItem(DynSvcOp operation)
    {
        StringBuilder sb = new();
        sb.AppendLine("{");
        for (int i = 0; i < operation.Parameters.Length; i++)
        {
            var parameter = operation.Parameters[i];

            string value =
                parameter.Type == "String" ? "\"\"" :
                parameter.Type == "Int" ? "0" :
                "{}";

            sb.AppendLine($"\t\"{parameter.Name}\": {value}{(i == operation.Parameters.Length - 1 ? "" : ',')}");
        }
        sb.AppendLine("}");

        PostmanBody body = new()
        {
            Mode = "raw",
            Raw = sb.ToString()
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
