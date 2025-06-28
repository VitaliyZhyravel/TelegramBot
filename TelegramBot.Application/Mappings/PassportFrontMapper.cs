using Mindee.Parsing.Generated;
using TelegramBotConsole;
using TelegramBotConsole.Models;

namespace TelegramBot.Application.Mappings
{
    public class PassportFrontMapper : IMapper<PassportFrontModel>
    {
        public OperationResult<PassportFrontModel> Map(Dictionary<string, GeneratedFeature> dataFromFile)
        {
            var passportFrontModel = new PassportFrontModel();

            foreach (var item in dataFromFile)
            {
                var obj = item.Value.FirstOrDefault();
                var key = item.Key;

                if (obj == null) return OperationResult<PassportFrontModel>.Failure($"Помилка зчитування данних {key}");

                switch (key)
                {
                    case "full_name":

                        passportFrontModel.FullName = new FullNames(
                            obj.TryGetString("nameua"),
                            obj.TryGetString("nameuk"));

                        break;
                    case "day_of_birth":

                        if (obj["value"].TryGetDateTime(out DateTime birthday))
                        {
                            passportFrontModel.DayOfBirth = birthday;
                        }
                        else
                        {
                            return OperationResult<PassportFrontModel>.Failure($"Помилка зчитування данних {key}");
                        }
                        break;
                    case "date_of_expiry":

                        if (obj["value"].TryGetDateTime(out DateTime dateExpiry))
                        {
                            passportFrontModel.DateOfExpiry = dateExpiry;
                        }
                        else
                        {
                            return OperationResult<PassportFrontModel>.Failure($"Помилка зчитування данних {key}");
                        }
                        break;
                    case "record_no":

                        passportFrontModel.RecordNo = obj.TryGetString("value");

                        break;

                    case "document_no":

                        passportFrontModel.DocumentNo = obj.TryGetString("value");

                        break;
                }
            }
            if (!passportFrontModel.IsValid)
            {
                return OperationResult<PassportFrontModel>.Failure("Model wasn`t created successfuly");
            }

            return OperationResult<PassportFrontModel>.Sucssecc(passportFrontModel);
        }
    }
}
