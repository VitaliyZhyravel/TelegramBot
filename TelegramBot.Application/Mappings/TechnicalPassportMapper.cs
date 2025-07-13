using Mindee.Product.Generated;
using TelegramBot.Domain.Domain;

namespace TelegramBot.Application.Mappings;

public static class TechnicalPassportMapper
{
    public static IDocumentData Map(GeneratedV1 dataFromFile)
    {
        var newTechPassport = new TechnicalPassportModel
        {
            VehicleIdentificationNumber = dataFromFile.Prediction.Fields["vehicle_identification_number"].FirstOrDefault()!.TryGetString("value"),
            Make = dataFromFile.Prediction.Fields["make"].FirstOrDefault()!.TryGetString("value"),
            Model = dataFromFile.Prediction.Fields["model"].FirstOrDefault()!.TryGetString("value"),
            BodyType = dataFromFile.Prediction.Fields["body_type"].FirstOrDefault()!.TryGetString("value")
        };

        return newTechPassport;
    }
}