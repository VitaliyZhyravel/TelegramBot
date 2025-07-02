using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBotConsole;

namespace TelegramBot.Application.Telegram.Handlers;

public class GreetingsHandler : IMessageHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IOpenAiService _openAiService;

    public GreetingsHandler(ITelegramBotClient botClient, IOpenAiService openAiService)
    {
        _botClient = botClient;
        _openAiService = openAiService;
    }

    public bool CanHandle(Message message) => message.Type == MessageType.Text && message.Text != null &&
        message.Text == "/start";

    public async Task HandleMessageAsync(Message message,CancellationToken cancellationToken)
    {
        var result = await _openAiService.GenerateGreetingsAsync();

        if (!result.IsSuccess)
        {
            await _botClient.SendMessage(message.Chat.Id, $"Вибач, щось пішло не так з OpenAI 😞\nError: {result.ErrorMesage}",cancellationToken : cancellationToken);
            return;
        }

        var greeting = result.Data!;
        long chatId = message.Chat.Id;


        await _botClient.SendMessage(chatId, greeting);
        await _botClient.SendMessage(chatId, "Оберіть дію:",

        replyMarkup: new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("📷 Надати паспорт") },
            new[] { new KeyboardButton("📷 Надати зворот паспорта") },
            new[] { new KeyboardButton("📷 Надати техпаспорт") },
            new[] { new KeyboardButton("✅ Згенерувати автостраховку") },
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        }, cancellationToken: cancellationToken);
    }
}
