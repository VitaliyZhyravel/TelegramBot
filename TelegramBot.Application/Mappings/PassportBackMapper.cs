using Mindee.Parsing.Generated;
using TelegramBot.Domain.Models;
using TelegramBotConsole;
using TelegramBotConsole.Models;

namespace TelegramBot.Application.Mappings;

public class PassportBackMapper 
{
    public OperationResultGeneric<IDocumentData> Map(Dictionary<string, GeneratedFeature> dataFromFile)
    {
        var passportBackModel = new PassportBackModel();

        foreach (var item in dataFromFile)
        {
            var obj = item.Value.FirstOrDefault();
            var key = item.Key;

            if (obj == null) return OperationResultGeneric<IDocumentData>.Failure($"Помилка зчитування данних {key}");

            switch (key)
            {
                case "authority":

                    passportBackModel.Authority = obj.TryGetString("value");

                    break;
                case "date_of_issue":

                    if (obj["value"].TryGetDateTime(out DateTime birthday))
                    {
                        passportBackModel.DateOfIssue = birthday;
                    }
                    else
                    {
                        return OperationResultGeneric<IDocumentData>.Failure($"Помилка зчитування данних {key}");
                    }
                    break;
                case "identification_code":

                    passportBackModel.IdentificationCode = obj.TryGetString("value");

                    break;

            }
        }
        if (!passportBackModel.IsValid)
        {
            return OperationResultGeneric<IDocumentData>.Failure("Model wasn`t created successfuly");
        }

        return OperationResultGeneric<IDocumentData>.Success(passportBackModel);
    }
}
