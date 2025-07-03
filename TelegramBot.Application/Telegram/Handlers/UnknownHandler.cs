using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Interfaces.Handlers;

namespace TelegramBot.Application.Telegram.Handlers
{
    public class UnknownHandler : IUnknownHandler
    {
        private readonly ITelegramBotClient _botClient;

        public UnknownHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task UnknownMessageHandlerAsync(Update update)
        {
            var chatId = update.Message?.Chat.Id;

            if (chatId != null)
            {
                await _botClient.SendMessage(chatId.Value, "Введений Вами текст, не зовсім зрозумілий, спробуйте \r\nскористатись кнопками, що виведені нижче. \r\n👇👇👇👇👇👇👇👇👇👇👇👇👇👇👇");
            }
        }
    }
}
