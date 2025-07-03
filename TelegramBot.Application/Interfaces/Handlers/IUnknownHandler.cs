using Telegram.Bot.Types;

namespace TelegramBot.Application.Interfaces.Handlers
{
    public interface IUnknownHandler
    {
        Task UnknownMessageHandlerAsync(Update update);
    }
}
