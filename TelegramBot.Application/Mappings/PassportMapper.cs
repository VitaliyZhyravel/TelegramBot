using Mindee.Parsing.V2.Field;
using TelegramBot.Domain.Models;

namespace TelegramBot.Application.Mappings;
    
public static class PassportMapper
{
    public static IDocumentData Map(InferenceFields fields)
    {
        return new PassportModel
        {
            Name = GetValue(fields, "given_names"),
            Surname = GetValue(fields, "surnames"),
            BirthDate = GetValue(fields, "date_of_birth"),
            Nationality = GetValue(fields, "nationality"),
            DocumentNumber = GetValue(fields, "document_number"),
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