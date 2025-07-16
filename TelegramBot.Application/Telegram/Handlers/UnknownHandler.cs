using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBotConsole.Enums;
using TelegramBotConsole.User;

namespace TelegramBot.Application.Telegram.Handlers
{
    public class UnknownHandler : IUnknownHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IOpenAiService openAiService;

        public UnknownHandler(ITelegramBotClient botClient, IOpenAiService openAiService)
        {
            _botClient = botClient;
            this.openAiService = openAiService;
        }

        public async Task UnknownMessageHandlerAsync(Message message, CancellationToken cancellationToken)
        {
            var chatId = message!.Chat.Id;
            var userSession = SessionStorage.GetSession(chatId);

            var result = await openAiService.GenerateReplyToUserQuestion(message?.Text!);

            if (!result.IsSuccess || result.Data == null)
            {
                await _botClient.SendMessage(chatId, "⚠️ Вибачте, щось пішло не так. Спробуйте ще раз пізніше", cancellationToken: cancellationToken);
            }

            var ending = GetStepInstruction(userSession.Step);
            await _botClient.SendMessage(chatId, result.Data + $"\n\r{ending}");
        }
        private static string GetStepInstruction(BotStep step)
        {
            return step switch
            {
                BotStep.Greeting => "🙌 Щоб почати роботу з ботом введіть команду /start",
                BotStep.Passport => "📷 Для продовження оформлення автостраховки — надайте фото паспорта.",
                BotStep.WaitingForConfirmPassport => "✅ Перевірте, чи всі дані з паспорта зчитано правильно.",
                BotStep.TechnicalPassport => "📷 Для продовження надайте фото техпаспорта.",
                BotStep.WaitingForConfirmTechnicalPassport => "✅ Перевірте, чи всі дані з техпаспорта зчитано правильно.",
                BotStep.GenerateInsurance => "💵 Підтвердіть вартість формування страховки"
            };
        }
    }
}
