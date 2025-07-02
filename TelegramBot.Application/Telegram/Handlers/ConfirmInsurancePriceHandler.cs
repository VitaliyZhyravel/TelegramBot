using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBotConsole.Enums;
using TelegramBotConsole.User;

namespace TelegramBot.Application.Telegram.Handlers;

public class ConfirmInsurancePriceHandler : ICallbackHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IOpenAiService _openAiService;
    private readonly IConfiguration configuration;
    private readonly IPdfGenerator pdfGenerator;

    public ConfirmInsurancePriceHandler(ITelegramBotClient botClient, IOpenAiService openAiService, IConfiguration configuration, IPdfGenerator pdfGenerator)
    {
        _botClient = botClient;
        _openAiService = openAiService;
        this.configuration = configuration;
        this.pdfGenerator = pdfGenerator;
    }

    public bool CanHandle(CallbackQuery callbackQuery) => callbackQuery.Message != null && callbackQuery.Message.Text != null &&
        (callbackQuery.Data == "✅ Так" || callbackQuery.Data == "❌ Ні") && SessionStorage.GetSession(callbackQuery.Message.Chat.Id).Step == BotStep.GenerateInsurance &&
        callbackQuery.Message.Text.Contains("Вартість автостраховки становить 100 доларів\nЧи підходить вам така ціна?");

    public async Task HandleMessageAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var chatId = callbackQuery!.Message!.Chat.Id;
        var userSession = SessionStorage.GetSession(chatId);

        if (callbackQuery.Data == "✅ Так")
        {
            var response = await _openAiService.GenerateInsuranceAsync(userSession);

            if (string.IsNullOrWhiteSpace(response.Data))
            {
                await _botClient.SendMessage(chatId, "Вибачте, але не вдалося згенерувати страховий поліс. Спробуйте ще раз пізніше.", cancellationToken: cancellationToken);
                throw new InvalidOperationException($"{response.ErrorMesage}");
            }

            var outPath = $"{configuration["DownloadingPaths:Insurance"]!}//{Guid.NewGuid()}.pdf";

            pdfGenerator.GeneratePdf(response.Data, outPath);

            var buffer = await File.ReadAllBytesAsync(outPath);
            await using var ms = new MemoryStream(buffer);
            await _botClient.SendDocument(chatId, InputFile.FromStream(ms, "Insurance.pdf"), cancellationToken: cancellationToken);

            userSession.Step = BotStep.None;
        }
        else if (callbackQuery.Data == "❌ Ні")
        {
            await _botClient.SendMessage(chatId, "Вибачте але 100$ це єдина доступна ціна", cancellationToken: cancellationToken);
            return;
        }
    }
}