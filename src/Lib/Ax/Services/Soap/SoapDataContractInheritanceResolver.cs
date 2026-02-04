using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal sealed class SoapDataContractInheritanceResolver
{
    private readonly ILogger _logger;

    public SoapDataContractInheritanceResolver(ILogger logger)
    {
        _logger = logger;
    }

    public InheritanceResolvedDataContract Resolve(IReadOnlyCollection<AxDataContractDefn> types, string name, HashSet<string>? stack = null)
    {
        stack ??= [];
        
        if (!stack.Add(name))
        {
            throw new InvalidOperationException($"Cycle in inheritance: {name}");
        }

        var def = types.FirstOrDefault(x => x.Name == name) ?? throw new InvalidOperationException($"Unknown type {name}");

        var props = new Dictionary<string, AxDataContractPropertyDefn>();

        if (!string.IsNullOrWhiteSpace(def.Extends))
        {
            var baseDef = types.FirstOrDefault(x => x.Name == def.Extends);

            if (baseDef is not null)
            {
                var baseResolved = Resolve(types, def.Extends, stack);

                foreach (var p in baseResolved.Properties)
                {
                    props[p.Name] = p;
                }
            }
            else
            {
                _logger.LogWarning("The base type {d} for type {t} is not known.", def.Extends, name);
            }
        }

        foreach (var p in def.Properties)
        {
            props[p.Name] = p;
        }

        stack.Remove(name);

        return new InheritanceResolvedDataContract
        {
            Name = name,
            Properties = props.Values.ToList()
        };
    }

}