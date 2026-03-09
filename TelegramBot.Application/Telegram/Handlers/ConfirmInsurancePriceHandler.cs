using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Interfaces;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Application.Rules;
using TelegramBot.Domain.Enums;
using TelegramBot.Domain.User;

namespace TelegramBot.Application.Telegram.Handlers;

public class ConfirmInsurancePriceHandler : ICallbackHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IConfiguration configuration;
    private readonly IPdfGenerator pdfGenerator;
    private readonly ILogger<ConfirmInsurancePriceHandler> logger;

    public ConfirmInsurancePriceHandler(ITelegramBotClient botClient, IConfiguration configuration, IPdfGenerator pdfGenerator,ILogger<ConfirmInsurancePriceHandler> logger)
    {
        _botClient = botClient;
        this.configuration = configuration;
        this.pdfGenerator = pdfGenerator;
        this.logger = logger;
    }

    public bool CanHandle(CallbackQuery callbackQuery) => callbackQuery.Message != null && callbackQuery.Message.Text != null &&
        (callbackQuery.Data == "✅ Так" || callbackQuery.Data == "❌ Ні") && SessionStorage.GetSession(callbackQuery.Message.Chat.Id).Step == BotStep.WaitingForConfirmPrice;

    public async Task HandleMessageAsync(CallbackQuery callbackQuery, long chatId, CancellationToken cancellationToken)
    {
        var userSession = SessionStorage.GetSession(chatId);

        if (callbackQuery.Data == "✅ Так")
        {
            logger.LogInformation($"Chat {chatId} confirmed insurance price");

            var outPath = $"{configuration["DownloadingPaths:Insurance"]!}//{Guid.NewGuid()}.pdf";

            pdfGenerator.GeneratePdf(Templates.TemplateForInsurance(userSession), outPath);

            var buffer = await File.ReadAllBytesAsync(outPath);
            await using var ms = new MemoryStream(buffer);

            await _botClient.SendMessage(chatId, "🎉 Ваш страховий поліс готовий!", cancellationToken: cancellationToken);
            await _botClient.SendDocument(chatId, InputFile.FromStream(ms, "Insurance.pdf"), cancellationToken: cancellationToken);
            
            userSession.Step = BotStep.Complete;
        }
        else if (callbackQuery.Data == "❌ Ні")
        {
            logger.LogInformation($"Chat {chatId} not confirmed insurance price");
            await _botClient.SendMessage(chatId, "Вибачте але 100$ це єдина доступна ціна", cancellationToken: cancellationToken);
        }
    }
}