using Telegram.Bot.Types;

namespace TelegramBot.Application.Interfaces.Handlers;

public interface IMessageHandler
{
    Task HandleMessageAsync(Message message, long chatId, CancellationToken cancellationToken);
    bool CanHandle(Message message);
}