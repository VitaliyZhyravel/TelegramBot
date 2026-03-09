using Mindee.Parsing.V2.Field;
using TelegramBot.Domain.Models;

namespace TelegramBot.Application.Mappings;

public static class TechnicalPassportMapper
{
    public static IDocumentData Map(InferenceFields fields)
    {
        return new TechnicalPassportModel
        {
            VehicleIdentificationNumber = GetValue(fields,"vehicle_identification_number"),
            Make = GetValue(fields,"make"),
            Model = GetValue(fields,"model"),
            BodyType = GetValue(fields,"body_type"),
        };
    }
    private static string? GetValue(InferenceFields fields, string key)
    {
        if (!fields.TryGetValue(key, out var field))
            return null;

        var value = field?.SimpleField?.Value?.ToString();

        return string.IsNullOrWhiteSpace(value) ? null : value?.Trim();
    }
}