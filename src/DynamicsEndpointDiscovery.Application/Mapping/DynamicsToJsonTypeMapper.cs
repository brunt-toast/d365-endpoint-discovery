namespace DynamicsEndpointDiscovery.Application.Mapping;

internal static class DynamicsToJsonTypeMapper
{
    public static string MapType(string type)
    {
        if (type.EndsWith("[]"))
        {
            return "array";
        }

        return type switch
        {
            "Boolean" => "bool",
            "Decimal" or "Double" or "Int32" or "Int64" => "number",
            "List`1" => "array",
            "String" or "DateTime" => "string",
            "Void" => "",
            _ => "object"
        };
    }

    public static string GetDefaultValue(string type)
    {
        if (type.EndsWith("[]"))
        {
            return "array";
        }

        return type switch
        {
            "Boolean" => "false",
            "Decimal" or "Double" or "Int32" or "Int64" => "0",
            "DateTime" => "\"1900-01-01T00:00Z\"",
            "List`1" => "[]",
            "String" => "\"\"",
            "Void" => "",
            _ => "{}"
        };
    }
}
