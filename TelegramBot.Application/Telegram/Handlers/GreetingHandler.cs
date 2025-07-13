using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBotConsole.User;

namespace TelegramBot.Application.Telegram.Handlers;

public class GreetingHandler : IMessageHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IOpenAiService _openAiService;
    private readonly ILogger<GreetingHandler> logger;

    public GreetingHandler(ITelegramBotClient botClient, IOpenAiService openAiService, ILogger<GreetingHandler> logger)
    {
        _botClient = botClient;
        _openAiService = openAiService;
        this.logger = logger;
    }

    public bool CanHandle(Message message) => message.Type == MessageType.Text && message.Text != null &&
        message.Text == "/start";

    public async Task HandleMessageAsync(Message message, long chatId, CancellationToken cancellationToken)
    {
        var greetingText = "👋 Вітаю! Я — Telegram-бот автострахування **PolisUa**\n\n" +

                "Допоможу швидко та зручно оформити автостраховку 🚗💼\n"+
                "📸 Для початку — надайте фото свого паспорта\n" +
                "Я зчитую інформацію автоматично та формую страховий поліс у PDF \n\n" +

                "✅ Це займе лише кілька хвилин";
       
            var session = SessionStorage.GetSession(chatId);
            session.Step = UserSession.GetNextStep(session.Step);

            await _botClient.SendMessage(chatId, greetingText, cancellationToken: cancellationToken);
    }
}
