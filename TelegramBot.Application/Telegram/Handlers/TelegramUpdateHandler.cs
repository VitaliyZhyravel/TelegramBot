using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces.Handlers;

namespace TelegramBot.Application.Telegram.Handlers;

public class TelegramUpdateHandler : Interfaces.Handlers.IUpdateHandler
{
    private readonly IEnumerable<IMessageHandler> messageHandlers;
    private readonly IEnumerable<ICallbackHandler> callbackHandlers;

    public TelegramUpdateHandler(IEnumerable<IMessageHandler> messageHandlers, IEnumerable<ICallbackHandler> callbackHandlers,
        ILogger<TelegramUpdateHandler> logger)
    {
        this.messageHandlers = messageHandlers;
        this.callbackHandlers = callbackHandlers;
    }

    public async Task HandleUpdateAsync( Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message != null)
        {
            var message = update.Message;

            foreach (var handler in messageHandlers)
            {
                if (handler.CanHandle(message))
                {
                    await handler.HandleMessageAsync(message, cancellationToken);
                }
            }
        }
        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
        {
            var callbackQuery = update.CallbackQuery;

            foreach (var handler in callbackHandlers)
            {
                if (handler.CanHandle(callbackQuery))
                {
                    await handler.HandleMessageAsync(callbackQuery, cancellationToken);
                }
            }
        }
        else
        {

        }
    }
}
