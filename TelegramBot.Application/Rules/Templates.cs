using System.Text;
using TelegramBot.Domain.User;

namespace TelegramBot.Application.Rules;

static class Templates
{
    public static string TemplateForInsurance(UserSession userSession) 
    {
        StringBuilder builder = new StringBuilder();

        if (userSession.Passport == null && userSession.TechnicalPassport == null) { return string.Empty; }

        builder.Append(
            $"Страховий поліс – №:{new Random().Next(100000, 200000)} \r\n\r\n" +
            $"Страхувальник:\r\n");

        builder.Append($"Прізвище та ім'я: {userSession.Passport!.Surname}\t {userSession.Passport.Name}\r\n" +
                       $"Дата народження: {userSession.Passport.BirthDate}\r\n" +
                       $"Документ No: {userSession.Passport.DocumentNumber} \r\n\n");

        builder.Append("Страхова компанія: \r\n" +
                       "Адреса: PolisUa\r\n" +
                       "Телефон: +380993652829\r\n" +
                       "Сайт: www.PolisUa.com\r\n\r\n" +

                       "Видано: МВС України\r\n\n" +

                       "ТРАНСПОРТНИЙ ЗАСІБ: \r\n");

        builder.Append($"VIN: {userSession.TechnicalPassport!.VehicleIdentificationNumber}\n" +
                       $"Модель: {userSession.TechnicalPassport.Model}\n" +
                       $"Марка: {userSession.TechnicalPassport.Make}\n" +
                       $"Тип двигуна: {userSession.TechnicalPassport.BodyType}\n\n");

        builder.Append(
            "СТРАХОВЕ ЗАБЕЗПЕЧЕННЯ: \r\n" +

            "Шкода життю і здоров’ю: <200 000 грн> \r\n" +
            "Шкода майну: <100 000 грн>\r\n " +
            "Франшиза: <50 000грн >\r\n\r\n" +

            "ДОДАТКОВА ІНФОРМАЦІЯ:\r\n" +
            "Цей документ є візуальною формою поліса, що підтверджує укладення внутрішнього електронного договору страхування.");

        return builder.ToString();  
    }
}