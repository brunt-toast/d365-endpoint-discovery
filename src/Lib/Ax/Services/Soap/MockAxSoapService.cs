using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal class MockAxSoapService : IAxSoapService
{
    private static readonly (string Language, string Contract, string Id, string Description, string EffectiveDate)[] LocalisationTemplates =
    [
        ("en-us", "{0}", "ID", "Description", "Effective date"),
        ("fr", "{0}", "Identifiant", "Description", "Date d'effet"),
        ("es", "{0}", "Identificador", "Descripción", "Fecha de vigencia")
    ];

    public Task<SoapTypeCollection> GetDataContractsForServices(IEnumerable<string> serviceNames)
    {
        var definitions = CreateDefinitions(serviceNames).ToArray();

        return Task.FromResult(new SoapTypeCollection
        {
            Samples = definitions.Select(x => new KeyValuePair<string, string>(x.Name, string.Empty)).ToDictionary(),
            Definitions = definitions,
            Localisations = CreateLocalisations(definitions).ToArray()
        });
    }

    private static IEnumerable<AxDataContractDefn> CreateDefinitions(IEnumerable<string> serviceNames)
    {
        foreach (var serviceName in serviceNames.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.Ordinal))
        {
            yield return new AxDataContractDefn
            {
                Name = $"{serviceName}Contract",
                Extends = string.Empty,
                LabelId = ToLabelId(serviceName, "Contract"),
                Properties =
                [
                    new AxDataContractPropertyDefn
                    {
                        Name = "id",
                        MinimumOccurances = 1,
                        MaximumOccurances = 1,
                        IsNullable = false,
                        Type = "guid",
                        LabelId = ToLabelId(serviceName, "Id")
                    },
                    new AxDataContractPropertyDefn
                    {
                        Name = "description",
                        MinimumOccurances = 0,
                        MaximumOccurances = 1,
                        IsNullable = true,
                        Type = "string",
                        LabelId = ToLabelId(serviceName, "Description")
                    },
                    new AxDataContractPropertyDefn
                    {
                        Name = "effective_date",
                        MinimumOccurances = 0,
                        MaximumOccurances = 1,
                        IsNullable = false,
                        Type = "dateTime",
                        LabelId = ToLabelId(serviceName, "EffectiveDate")
                    }
                ]
            };
        }
    }

    private static IEnumerable<AxLabelLocalisation> CreateLocalisations(IEnumerable<AxDataContractDefn> definitions)
    {
        foreach (var definition in definitions)
        {
            var contractDisplayName = ToDisplayName(definition.Name);
            foreach (var template in LocalisationTemplates)
            {
                yield return new AxLabelLocalisation
                {
                    LabelId = definition.LabelId,
                    Language = template.Language,
                    Value = string.Format(template.Contract, contractDisplayName)
                };

                foreach (var property in definition.Properties)
                {
                    yield return new AxLabelLocalisation
                    {
                        LabelId = property.LabelId,
                        Language = template.Language,
                        Value = property.Name switch
                        {
                            "id" => template.Id,
                            "description" => template.Description,
                            "effective_date" => template.EffectiveDate,
                            _ => ToDisplayName(property.Name)
                        }
                    };
                }
            }
        }
    }

    private static string ToLabelId(string serviceName, string labelName)
    {
        return $"@DED_{serviceName}:{labelName}";
    }

    private static string ToDisplayName(string value)
    {
        return string.Concat(value.Select((x, i) =>
            i > 0 && char.IsUpper(x)
                ? $" {x}"
                : x.ToString())).Replace("_", " ");
    }
}
