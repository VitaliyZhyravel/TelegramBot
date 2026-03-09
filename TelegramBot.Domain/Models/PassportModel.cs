using System.Text;

namespace TelegramBot.Domain.Models;

public class PassportModel : IDocumentData
{
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string? BirthDate { get; init; }
    public string? Nationality { get; init; }
    public string? DocumentNumber { get; init; }

    public override string ToString()
    {
        var fields = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(Name))
            fields.Append($"Ім'я: {Name}\n");

        if (!string.IsNullOrWhiteSpace(Surname))
            fields.Append($"Прізвище: {Surname}\n");

        if (!string.IsNullOrWhiteSpace(BirthDate))
            fields.Append($"Дата народження: {BirthDate}\n");

        if (!string.IsNullOrWhiteSpace(Nationality))
            fields.Append($"Національність: {Nationality}\n");

        if (!string.IsNullOrWhiteSpace(DocumentNumber))
            fields.Append($"Номер документа: {DocumentNumber}\n");

        return fields.ToString();
    }
}

