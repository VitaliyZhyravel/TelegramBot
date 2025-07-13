using TelegramBot.Domain.Domain;

namespace TelegramBot.Application.Mappings;

public class PassportModel : IDocumentData
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? BirthDate { get; set; }
    public string? BirthPlace { get; set; }
    public string? Nationality { get; set; }
    public string? MyProperty { get; set; }
    public string? Sex { get; set; }
    public string? DocumentNumber { get; set; }
    public string? IssueDate { get; set; }
    public string? ExpiryDate { get; set; }

    public override string ToString()
    {
        var fields = new List<string>();

        if (!string.IsNullOrWhiteSpace(Name))
            fields.Add($"Ім'я: {Name}");

        if (!string.IsNullOrWhiteSpace(Surname))
            fields.Add($"Прізвище: {Surname}");

        if (!string.IsNullOrWhiteSpace(BirthDate))
            fields.Add($"Дата народження: {BirthDate}");

        if (!string.IsNullOrWhiteSpace(BirthPlace))
            fields.Add($"Місце народження: {BirthPlace}");

        if (!string.IsNullOrWhiteSpace(Nationality))
            fields.Add($"Національність: {Nationality}");

        if (!string.IsNullOrWhiteSpace(Sex))
            fields.Add($"Стать: {Sex}");

        if (!string.IsNullOrWhiteSpace(DocumentNumber))
            fields.Add($"Номер документа: {DocumentNumber}");

        if (!string.IsNullOrWhiteSpace(IssueDate))
            fields.Add($"Дата видачі: {IssueDate}");

        if (!string.IsNullOrWhiteSpace(ExpiryDate))
            fields.Add($"Дата закінчення строку дії: {ExpiryDate}");

        return string.Join("\n", fields);
    }
}

