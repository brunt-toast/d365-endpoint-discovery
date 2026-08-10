using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Metadata;

internal class AxMetadataLabelService
{
    private const string DefaultLanguage = "en-us";

    private readonly AxCallingService _axCalling;
    private readonly ILogger _logger;

    public AxMetadataLabelService(AxCallingService axCalling, ILogger logger)
    {
        _axCalling = axCalling;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<AxLabelLocalisation>> GetLabels(IEnumerable<string> labelIds)
    {
        var distinctLabelIds = labelIds
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (distinctLabelIds.Length == 0)
        {
            _logger.LogWarning("No label metadata requests were made because no label IDs were found in the service metadata.");
            return [];
        }

        var labels = await Task.WhenAll(distinctLabelIds.Select(x => GetLabel(x, DefaultLanguage)));
        var resolvedLabels = labels.Where(x => x is not null).Cast<AxLabelLocalisation>().ToArray();

        if (resolvedLabels.Length == 0)
        {
            _logger.LogWarning("Label metadata requests completed but no label values were resolved for {labelCount} label IDs.", distinctLabelIds.Length);
        }

        return resolvedLabels;
    }

    private async Task<AxLabelLocalisation?> GetLabel(string labelId, string language)
    {
        var endpoint =
            $"/Metadata/Labels(Id='{Uri.EscapeDataString(labelId)}',Language='{Uri.EscapeDataString(language)}')";
        var response = await _axCalling.GetHttp(endpoint);

        if (string.IsNullOrWhiteSpace(response))
        {
            _logger.LogWarning("Label metadata request returned no content for label {labelId} and language {language}.", labelId, language);
            return null;
        }

        try
        {
            var json = JObject.Parse(response);
            var value =
                (string?)json["Label"] ??
                (string?)json["Value"] ??
                (string?)json["Text"];

            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogWarning("Label metadata request returned no label value for label {labelId} and language {language}.", labelId, language);
                return null;
            }

            return new AxLabelLocalisation
            {
                LabelId = labelId,
                Language = language,
                Value = value
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while parsing label {labelId} for language {language}: {ex}", labelId, language, ex.Message);
            return null;
        }
    }
}
