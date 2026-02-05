using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;
using System;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Xpp;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal static class DefaultValueGenerator
{
    public static object? Generate(TypeNode node)
    {
        return node.IsCollection 
            ? new[] { GenerateSingle(node) } 
            : GenerateSingle(node);
    }

    private static object? GenerateSingle(TypeNode node)
    {
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
            "datetime" => (DateTime)XppDateTime.MaxValue,
            "decimal" => decimal.MaxValue,
            "double" => double.MaxValue,
            "float" => float.MaxValue,
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