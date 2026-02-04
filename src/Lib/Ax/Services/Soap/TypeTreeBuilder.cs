using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal sealed class TypeTreeBuilder
{
    private readonly SoapDataContractInheritanceResolver _inheritance;

    public TypeTreeBuilder(SoapDataContractInheritanceResolver inheritance)
    {
        _inheritance = inheritance;
    }

    public TypeNode Build(IReadOnlyCollection<AxDataContractDefn> types, string typeName, HashSet<string>? stack = null)
    {
        stack ??= [];

        if (SoapPrimitiveTypes.IsPrimitive(typeName))
        {
            return new TypeNode
            {
                TypeName = typeName,
                IsPrimitive = true
            };
        }

        if (!stack.Add(typeName))
        {
            return new TypeNode
            {
                TypeName = typeName,
                Properties = []
            };
        }

        var def = types.FirstOrDefault(x => x.Name == typeName);

        if (def is null)
        {
            return new TypeNode
            {
                TypeName = typeName,
                Properties = []
            };
        }

        var resolved = _inheritance.Resolve(types, typeName);

        var props = new Dictionary<string, TypeNode>();

        foreach (var p in resolved.Properties)
        {
            props[p.Name] = Build(types, p.Type, stack);
            //props[p.Name].IsNullable = p.IsNullable;
        }

        stack.Remove(typeName);

        return new TypeNode
        {
            TypeName = typeName,
            Properties = props
        };
    }
}