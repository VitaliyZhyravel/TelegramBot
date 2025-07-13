using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces.Handlers;

namespace TelegramBot.Application.Telegram.Handlers;

public class TelegramUpdateHandler : IUpdateHandler
{
    private readonly IEnumerable<IMessageHandler> messageHandlers;
    private readonly IEnumerable<ICallbackHandler> callbackHandlers;
    private readonly IUnknownHandler unknownHandler;

    public TelegramUpdateHandler(IEnumerable<IMessageHandler> messageHandlers, IEnumerable<ICallbackHandler> callbackHandlers,
        IUnknownHandler unknownHandler)
    {
        this.messageHandlers = messageHandlers;
        this.callbackHandlers = callbackHandlers;
        this.unknownHandler = unknownHandler;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
       

        if (update.Type == UpdateType.Message && update.Message != null)
        {
            var chatId = update.Message!.Chat.Id;

            var message = update.Message;
            var isHandled = false;

            foreach (var handler in messageHandlers)
            {
                if (handler.CanHandle(message))
                {
                    await handler.HandleMessageAsync(message, chatId, cancellationToken);
                    isHandled = true;
                    break;
                }
            }
            if (!isHandled)
                await unknownHandler.UnknownMessageHandlerAsync(update.Message,cancellationToken);
        }
        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
        {
            var chatId = update.CallbackQuery.Message!.Chat.Id;

            var callbackQuery = update.CallbackQuery;

            foreach (var handler in callbackHandlers)
            {
                if (handler.CanHandle(callbackQuery))
                {
                    await handler.HandleMessageAsync(callbackQuery, chatId, cancellationToken);
                }
            }
        }
    }
}
