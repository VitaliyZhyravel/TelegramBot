using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBotConsole.User;

namespace TelegramBotConsole.Services;

public class OpenAiBetalgoiService : IOpenAiService
{
    private readonly OpenAIService _openAi;

    public OpenAiBetalgoiService(OpenAIService openAi)
    {
        _openAi = openAi;
    }

    public async Task<OperationResultGeneric<string>> GenerateGreetingsAsync()
    {
        var response = await _openAi.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("Ти — Telegram-бот, який вітає користувача і коротко пояснює, що потрібно надіслати фото документів. Згадай, що для цього потрібно скористатися кнопками. Пиши українською, коротко і дружньо."),

                ChatMessage.FromUser("Згенеруй коротке привітання для Telegram-бота автострахування. Приклад:\r\n\r\n" +
                "👋 Привіт! Я — бот для оформлення автострахування. 📷 Надішліть фото паспорта та техпаспорта за допомогою кнопок нижче 🚗" )
            },
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo
        });

        if (response.Successful)
        {
            var responseMessage = response.Choices.FirstOrDefault()?.Message?.Content;
            if (responseMessage != null)
            {
                return OperationResultGeneric<string>.Success(responseMessage);
            }
        }

        return OperationResultGeneric<string>.Failure($"Вибач, щось пішло не так з OpenAI 😞\nError: {response.Error}");
    }

    public async Task<OperationResultGeneric<string>> GenerateInsuranceAsync(UserSession userSession)
    {

        var response = await _openAi.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("Ти — помічник, який генерує приклади текстів страхових полісів для навчальних або демонстраційних проектів. " +
                "Твоє завдання — створити фіктивний текст автостраховки  по шаблону наданому нижче."),

                ChatMessage.FromUser("Згенеруй фіктивний текст автострахового полісу для PDF (не PDF-файл) На основі наданого нижче шаблона." +
                "\r\nМета — використання у навчальному проекті \r\n" +

                $"📄 Страховий поліс – №:{new Random().Next(100000,200000)} \r\n\r\n" +

                $"👤 Страхувальник:\r\n" +
                $"Прізвище та ім'я: {userSession.CarRegistration?.FullName?.NameUa}\r\n" +
                "Телефон:  +380987654321  \r\n" +
                "Email: userexample@gmail.com\r\n" +
                $"ІПН: {userSession.PassportBack?.IdentificationCode}\r\n" +
                "Адреса: Київ Україна\r\n" +
                $"Дата народження: {userSession.PassportFront?.DayOfBirth.ToShortDateString()}\r\n" +
                $"Документ No: {userSession.PassportFront?.DocumentNo} \r\n" +
                $"Record No: {userSession.PassportFront?.RecordNo} \r\n\r\n" +

                "🏢 Страхова компанія: \r\n" +
                "Адреса: PolisUa\r\n" +
                "Телефон: +380993652829\r\n" +
                "Сайт: www.PolisUa.com\r\n\r\n" +

                $"🚘 ТехПаспорт - №: {new Random().Next(1000000,1200000)}\r\n" +

                $"📅 Строк дії: з {DateTime.Now.ToShortDateString()} по {DateTime.Now.AddYears(2).ToShortDateString()}\r\n" +
                "📍 Видано: МВС України\r\n" +
                $"📆 Дата першої реєстрації: {userSession.CarRegistration?.DateOfRegistration?.dateOfFirstRegistration.ToShortDateString()}\r\n\r\n" +

                "🚗 ТРАНСПОРТНИЙ ЗАСІБ: \r\n" +

                "Модель: Mazda\r\n" +
                $"Рік: {userSession.CarRegistration?.YearOfManufacture}\r\n" +
                $"Держ. номер: {userSession.CarRegistration?.RegistrationNumber} \r\n" +
                "Тип: Легковий автомобіль \r\n " +
                "Місце реєстрації: Київ \r\n\r\n" +

                "💼 СТРАХОВЕ ЗАБЕЗПЕЧЕННЯ: \r\n" +

                "Шкода життю і здоров’ю: <200 000 грн> \r\n" +
                "Шкода майну: <100 000 грн>\r\n " +
                "Франшиза: <50 000грн >\r\n\r\n" +

                "📝 ДОДАТКОВА ІНФОРМАЦІЯ:\r\n" +
                "Цей документ є візуальною формою поліса, що підтверджує укладення внутрішнього електронного договору страхування."+
                "Підставляючи надані дані:")
            },
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo
        });

        if (response.Successful)
        {
            var responseMessage = response.Choices.FirstOrDefault()?.Message?.Content;

            if (responseMessage != null)
            {
                return OperationResultGeneric<string>.Success(responseMessage);
            }
        }

        return OperationResultGeneric<string>.Failure($"Вибач, щось пішло не так з OpenAI 😞\nError: {response.Error}");
    }
}
