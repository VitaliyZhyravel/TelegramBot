using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Domain.Enums;
using TelegramBot.Domain.User;

namespace TelegramBot.Application.Telegram.Handlers;

public class GreetingHandler : IMessageHandler
{
    private readonly ITelegramBotClient _botClient;

    public GreetingHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public bool CanHandle(Message message) => message.Type == MessageType.Text && message.Text != null &&
        message.Text == "/start";

    public async Task HandleMessageAsync(Message message, long chatId, CancellationToken cancellationToken)
    {
        var session = SessionStorage.GetSession(chatId);

        session.Step = BotStep.Greeting;    

        var greetingText = "👋 Вітаю! Я — Telegram-бот автострахування **PolisUa**\n\n" +

                "Допоможу швидко та зручно оформити автостраховку 🚗💼\n"+
                "📸 Для початку — надайте фото свого паспорта\n" +
                "Я зчитую інформацію автоматично та формую страховий поліс у PDF \n\n" +

                "✅ Це займе лише кілька хвилин";
       
            
            session.Step = UserSession.GetNextStep(session.Step);

            await _botClient.SendMessage(chatId, greetingText, cancellationToken: cancellationToken);
    }
}
