using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBotConsole.Enums;
using TelegramBotConsole.User;

namespace TelegramBot.Application.Telegram.Handlers;

public class DocumentSelectionHandler : IMessageHandler
{
    private readonly ITelegramBotClient _botClient;

    public DocumentSelectionHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    private static readonly HashSet<string> AllowedCommands = new()
    {
        "📷 Надати паспорт",
        "📷 Надати зворот паспорта",
        "📷 Надати техпаспорт",
        "✅ Згенерувати автостраховку"
    };

    public bool CanHandle(Message message) => message != null && message.Type == MessageType.Text && message.Text != null &&
       AllowedCommands.Contains(message.Text!);

    public async Task HandleMessageAsync(Message message,CancellationToken  cancellationToken)
    {
        var chatId = message.Chat.Id;
        var userSession = SessionStorage.GetSession(chatId);

        switch (message.Text)
        {
            case "📷 Надати паспорт":

                userSession.Step = BotStep.PassportFront;
                await _botClient.SendMessage(chatId, "Будь ласка, надішліть фото паспорта", cancellationToken: cancellationToken);

                break;
            case "📷 Надати зворот паспорта":

                userSession.Step = BotStep.PassportBack;
                await _botClient.SendMessage(chatId, "Будь ласка, надішліть зворот паспорта", cancellationToken: cancellationToken);

                break;
            case "📷 Надати техпаспорт":

                userSession.Step = BotStep.TechnicalPassport;
                await _botClient.SendMessage(chatId, "Будь ласка, надішліть техпаспорт", cancellationToken: cancellationToken);

                break;
            case "✅ Згенерувати автостраховку":

                var errors = new List<string>();

                if (userSession.CarRegistration == null) errors.Add("Техпаспорт не надано");
                if (userSession.PassportFront == null) errors.Add("Передню частину паспорта не надано");
                if (userSession.PassportBack == null) errors.Add("Задню частину паспорта не надано");

                if (userSession.CarRegistration != null && userSession.CarRegistration.IsConfirmed == false) errors.Add("Техпаспорт не підтверджено");
                if (userSession.PassportFront != null && userSession.PassportFront.IsConfirmed == false) errors.Add("Передню частину паспорта не підтверджено");
                if (userSession.PassportBack != null && userSession.PassportBack.IsConfirmed == false) errors.Add("Задню частину паспорта не підтверджено");

                if (errors.Count > 0)
                {
                    await _botClient.SendMessage(chatId, string.Join(Environment.NewLine, errors), cancellationToken: cancellationToken);
                }
                else
                {
                    userSession.Step = BotStep.GenerateInsurance;

                    var text = "Вартість автостраховки становить 100 доларів\nЧи підходить вам така ціна?";

                    await _botClient.SendMessage(chatId, text, replyMarkup: new InlineKeyboardButton[] { "✅ Так", "❌ Ні" }, cancellationToken: cancellationToken);
                }
                break;
        }
    }
}
