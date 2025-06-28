using Mindee.Parsing.Generated;
using TelegramBotConsole;
using TelegramBotConsole.Models;

namespace TelegramBot.Application.Mappings;

internal class PassportBackMapper : IMapper<PassportBackModel>
{
    public OperationResult<PassportBackModel> Map(Dictionary<string, GeneratedFeature> dataFromFile)
    {
        var passportBackModel = new PassportBackModel();

        foreach (var item in dataFromFile)
        {
            var obj = item.Value.FirstOrDefault();
            var key = item.Key;

            if (obj == null) return OperationResult<PassportBackModel>.Failure($"Помилка зчитування данних {key}");

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
                        return OperationResult<PassportBackModel>.Failure($"Помилка зчитування данних {key}");
                    }
                    break;
                case "identification_code":

                    passportBackModel.IdentificationCode = obj.TryGetString("value");

                    break;

            }
        }
        if (!passportBackModel.IsValid)
        {
            return OperationResult<PassportBackModel>.Failure("Model wasn`t created successfuly");
        }

        return OperationResult<PassportBackModel>.Sucssecc(passportBackModel);
    }
}
