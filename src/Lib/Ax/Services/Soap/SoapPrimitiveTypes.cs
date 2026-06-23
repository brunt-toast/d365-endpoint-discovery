namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

public static class SoapPrimitiveTypes
{
    private static readonly HashSet<string> Names = 
    [
        "anyURI",
        "boolean",
        "byte",
        "datetime",
        "decimal",
        "double",
        "float",
        "int",
        "long",
        "string",
        "unsignedByte",
        "unsignedInt",
        "unsignedLong",
        "unsignedShort",
        "guid"
    ];

    public static bool IsPrimitive(string name) => Names.Contains(name.ToLowerInvariant());
}
