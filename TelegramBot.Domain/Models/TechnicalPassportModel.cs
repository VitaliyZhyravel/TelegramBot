using System.Text;

namespace TelegramBot.Domain.Models;

public class TechnicalPassportModel : IDocumentData
{
    public string? VehicleIdentificationNumber { get; init; }
    public string? Make { get; init; }
    public string? Model { get; init; }
    public string? BodyType { get; init; }

    public override string ToString()
    {
        var fields = new StringBuilder();

        if (!string.IsNullOrEmpty(VehicleIdentificationNumber))
            fields.Append($"VIN: {VehicleIdentificationNumber}\n");

        if (!string.IsNullOrEmpty(Make))
            fields.Append($"Марка: {Make}\n");

        if (!string.IsNullOrEmpty(Model))
            fields.Append($"Модель: {Model}\n");

        if (!string.IsNullOrEmpty(BodyType))
            fields.Append($"Тип кузова: {BodyType}\n");

        return fields.ToString();
    }
}
