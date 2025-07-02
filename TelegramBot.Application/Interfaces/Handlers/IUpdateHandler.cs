using Telegram.Bot.Types;

namespace TelegramBot.Application.Interfaces.Handlers;

public interface IUpdateHandler
{
    Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
}
