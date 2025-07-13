using TelegramBot.Domain.Domain;

namespace TelegramBot.Application.Mappings;

public class TechnicalPassportModel : IDocumentData
{
    public string? VehicleIdentificationNumber { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? BodyType { get; set; }

    public override string ToString()
    {
        var text = new List<string>();

        if (!string.IsNullOrEmpty(VehicleIdentificationNumber))
            text.Add($"VIN: {VehicleIdentificationNumber}");

        if (!string.IsNullOrEmpty(Make))
            text.Add($"Марка: {Make}");

        if (!string.IsNullOrEmpty(Model))
            text.Add($"Модель: {Model}");

        if (!string.IsNullOrEmpty(BodyType))
            text.Add($"Тип кузова: {BodyType}");

        return string.Join("\n", text);
        
    }
}
