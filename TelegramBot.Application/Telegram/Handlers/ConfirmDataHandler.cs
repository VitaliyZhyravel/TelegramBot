using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBotConsole.Enums;
using TelegramBotConsole.User;

namespace TelegramBot.Application.Telegram.Handlers;

public class ConfirmDataHandler : ICallbackHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<ConfirmDataHandler> logger;

    public ConfirmDataHandler(ITelegramBotClient botClient,ILogger<ConfirmDataHandler> logger)
    {
        _botClient = botClient;
        this.logger = logger;
    }

    public bool CanHandle(CallbackQuery callbackQuery) =>
        callbackQuery.Message?.Text != null &&
        (callbackQuery.Data == "✅ Так" || callbackQuery.Data == "❌ Ні") &&
        callbackQuery.Message.Text.Contains("Перевірте, чи всі дані правильні:");

    public async Task HandleMessageAsync(CallbackQuery callbackQuery, long chatId, CancellationToken cancellationToken)
    {
        var userSession = SessionStorage.GetSession(chatId);

        if (callbackQuery.Data == "✅ Так")
        {
            await _botClient.SendMessage(chatId, "✅ Дякуємо! Ваші дані підтверджені.", cancellationToken: cancellationToken);

            if (userSession.Step == BotStep.WaitingForConfirmPassport)
            {
                await _botClient.SendMessage(chatId, "📷 Для продовження оформлення надайте фото техпаспорта", cancellationToken: cancellationToken);
                logger.LogInformation($"Chat {chatId} confirmed data: Passport");
            }
            else if (userSession.Step == BotStep.WaitingForConfirmTechnicalPassport)
            {
                await _botClient.SendMessage(chatId, "💵 Вартість автострахування — 100$. Чи підходить вам така ціна?", replyMarkup: new InlineKeyboardButton[] { "✅ Так", "❌ Ні" }, cancellationToken: cancellationToken);
                logger.LogInformation($"Chat {chatId} confirmed data: Technical Passport");
            }

            userSession.Step = UserSession.GetNextStep(userSession.Step);
        }
        else if (callbackQuery.Data == "❌ Ні")
        {
            userSession.Step = UserSession.GetPreviousStep(userSession.Step);
            await _botClient.SendMessage(chatId, "🔁 Будь ласка, надішліть фото ще раз.", cancellationToken: cancellationToken);
            logger.LogInformation($"Chat {chatId} not confirmed data ");
        }
    }
}