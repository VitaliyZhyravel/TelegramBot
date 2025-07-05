using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBotConsole.Enums;
using TelegramBotConsole.User;

namespace TelegramBot.Application.Telegram.Handlers;

public class ConfirmDataHandler : ICallbackHandler
{
    private readonly ITelegramBotClient _botClient;

    public ConfirmDataHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public bool CanHandle(CallbackQuery callbackQuery) =>
        callbackQuery.Message != null &&
        callbackQuery.Message.Text != null &&
        (callbackQuery.Data == "✅ Так" || callbackQuery.Data == "❌ Ні")&&
        callbackQuery.Message.Text.Contains("Перевірте, чи всі дані правильні:");

    public async Task HandleMessageAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var chatId = callbackQuery!.Message!.Chat.Id;
        var userSession = SessionStorage.GetSession(chatId);

        if (callbackQuery.Data == "✅ Так")
        {
            switch (userSession.Step)
            {
                case BotStep.PassportFront: userSession.PassportFront!.IsConfirmed = true; break;
                case BotStep.PassportBack: userSession.PassportBack!.IsConfirmed = true; break;
                case BotStep.TechnicalPassport: userSession.CarRegistration!.IsConfirmed = true; break;
            }
            await _botClient.SendMessage(chatId, "Дякуємо! Ваші дані успішно збережено.", cancellationToken : cancellationToken);

            userSession.Step = BotStep.None;
        }
        else if (callbackQuery.Data == "❌ Ні")
        {
            await _botClient.SendMessage(chatId, "Будь ласка, надішліть фото ще раз.", cancellationToken: cancellationToken);
        }
    }
}
