
using Mindee.Parsing.Generated;
using TelegramBotConsole;
using TelegramBotConsole.Models;

namespace TelegramBot.Application.Mappings
{
    internal class CarRegistrationMapper : IMapper<CarRegistrationModel>
    {
        public OperationResult<CarRegistrationModel> Map(Dictionary<string, GeneratedFeature> dataFromFile)
        {
            var vehicleRegistrationModel = new CarRegistrationModel();

            foreach (var item in dataFromFile)
            {
                var obj = item.Value.FirstOrDefault();
                var key = item.Key;

                if (obj == null) throw new ArgumentException($"Помилка зчитування данних об'єкт не знайдено");

                switch (key)
                {
                    case "full_name":

                        vehicleRegistrationModel.FullName = new FullNames(
                            obj.TryGetString("nameua"),
                            obj.TryGetString("nameuk"));

                        break;
                    case "dates_of_registration":

                        if (obj["date_of_registration"].TryGetDateTime(out DateTime date_of_registration) &&
                            obj["first_date_of_registration"].TryGetDateTime(out DateTime first_date_of_registration))
                        {
                            vehicleRegistrationModel.DateOfRegistration = new DatesOfRegistration
                            (date_of_registration, first_date_of_registration);
                        }
                        else
                        {
                            throw new ArgumentException($"Помилка зчитування данних {key}");
                        }
                        break;
                    case "registration_number":

                        vehicleRegistrationModel.RegistrationNumber = obj.TryGetString("value");

                        break;

                    case "year_of_manufacture":

                        vehicleRegistrationModel.YearOfManufacture = obj.TryGetString("value");

                        break;
                }
            }
            if (!vehicleRegistrationModel.IsValid)
            {
                throw new Exception($"{nameof(CarRegistrationModel)} wasn`t created successfuly");
            }

            return OperationResult<CarRegistrationModel>.Sucssecc( vehicleRegistrationModel);
        }
    }
}