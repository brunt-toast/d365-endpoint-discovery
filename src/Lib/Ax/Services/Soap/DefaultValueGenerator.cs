using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal static class DefaultValueGenerator
{
    public static object? Generate(TypeNode node)
    {
        //if (node.IsNullable)
        //{
        //    return null;
        //}

        if (node.IsPrimitive)
        {
            return PrimitiveDefault(node.TypeName);
        }

        if (node.Properties is null || node.Properties.Count == 0)
        {
            return new Dictionary<string, object?>();
        }

        var obj = new Dictionary<string, object?>();

        foreach (var (name, child) in node.Properties)
        {
            obj[name] = Generate(child);
        }

        return obj;
    }

    private static object PrimitiveDefault(string type) =>
        type.ToLowerInvariant() switch
        {
            "anyURI" => "https://example.com",
            "boolean" => false,
            "byte" => sbyte.MaxValue,
            "datetime" => "2000-01-01T00:00Z",
            "decimal" => 0.1m,
            "double" => 0.1d,
            "float" => 0.1f,
            "int" => int.MaxValue,
            "long" => long.MaxValue,
            "string" => string.Empty,
            "unsignedByte" => byte.MaxValue,
            "unsignedInt" => uint.MaxValue,
            "unsignedLong" => ulong.MaxValue,
            "unsignedShort" => ushort.MaxValue,
            "guid" => Guid.AllBitsSet,
            _ => $"[Unknown type {type}]"
        };
}