using Mindee.Product.InternationalId;
using TelegramBot.Domain.Domain;
using TelegramBotConsole;

namespace TelegramBot.Application.Mappings;

public static class PassportMapper
{
    public static IDocumentData Map(InternationalIdV2Document dataFromFile)
    {
        PassportModel passportModel = new PassportModel();

        if (dataFromFile.GivenNames?.Any(g => !string.IsNullOrWhiteSpace(g.Value)) == true)
            passportModel.Name = string.Join(" ", dataFromFile.GivenNames.Select(f => f.Value));

        if (dataFromFile.Surnames?.Any(s => !string.IsNullOrWhiteSpace(s.Value)) == true)
            passportModel.Surname = string.Join(" ", dataFromFile.Surnames.Select(f => f.Value));

        if (!string.IsNullOrWhiteSpace(dataFromFile.BirthDate?.Value))
            passportModel.BirthDate = dataFromFile.BirthDate.Value;

        if (!string.IsNullOrWhiteSpace(dataFromFile.BirthPlace?.Value))
            passportModel.BirthPlace = dataFromFile.BirthPlace.Value;

        if (!string.IsNullOrWhiteSpace(dataFromFile.Nationality?.Value))
            passportModel.Nationality = dataFromFile.Nationality.Value;

        if (!string.IsNullOrWhiteSpace(dataFromFile.Sex?.Value))
            passportModel.Sex = dataFromFile.Sex.Value;

        if (!string.IsNullOrWhiteSpace(dataFromFile.DocumentNumber?.Value))
            passportModel.DocumentNumber = dataFromFile.DocumentNumber.Value;

        if (!string.IsNullOrWhiteSpace(dataFromFile.IssueDate?.Value))
            passportModel.IssueDate = dataFromFile.IssueDate.Value;

        if (!string.IsNullOrWhiteSpace(dataFromFile.ExpiryDate?.Value))
            passportModel.ExpiryDate = dataFromFile.ExpiryDate.Value;

        return passportModel;
    }
}
