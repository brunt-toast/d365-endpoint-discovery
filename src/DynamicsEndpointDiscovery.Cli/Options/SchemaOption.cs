using System.CommandLine;
using System.CommandLine.Parsing;
using System.Reflection;
using DynamicsEndpointDiscovery.Cli.Attributes;
using DynamicsEndpointDiscovery.Cli.Enums;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class SchemaOption : Option<OutputSchemas>
{
    public SchemaOption() : base("--schema")
    {
        Description = "Specify the output schema.";
        Validators.Add(IsImplementedValidator);
    }

    private void IsImplementedValidator(OptionResult opt)
    {
        var value = opt.GetValue(this);

        if (value.GetType()
                .GetMember(value.ToString())
                .First()
                .GetCustomAttribute<NotImplementedAttribute>()
            is not null)
        {
            opt.AddError($"The strategy {value} isn't supported in this version.");
        }
    }
}
