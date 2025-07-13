using Telegram.Bot.Types;

namespace TelegramBot.Application.Interfaces.Handlers;

public interface ICallbackHandler
{
    Task HandleMessageAsync(CallbackQuery callbackQuery, long chatId, CancellationToken cancellationToken);
    bool CanHandle(CallbackQuery callbackQuery);
}
