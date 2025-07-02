using Telegram.Bot.Types;

namespace TelegramBot.Application.Interfaces.Handlers;

public interface IMessageHandler
{
    Task HandleMessageAsync(Message message, CancellationToken cancellationToken);

    bool CanHandle(Message message);
}