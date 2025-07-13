using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Application.Mappings;
using TelegramBot.Domain.Domain;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBotConsole;
using TelegramBotConsole.Enums;
using TelegramBotConsole.User;

namespace TelegramBot.Application.Telegram.Handlers;

public class DocumentsSubmissionHandler : IMessageHandler
{
    private readonly Interfaces.IFileService _fileService;
    private readonly ITelegramBotClient _botClient;
    private readonly IConfiguration _configuration;
    private readonly IMindeeService _mindeeService;
    private readonly ILogger<DocumentsSubmissionHandler> logger;

    public DocumentsSubmissionHandler(Interfaces.IFileService fileService, ITelegramBotClient botClient,
        IConfiguration configuration, IMindeeService mindeeService, ILogger<DocumentsSubmissionHandler> logger)
    {
        _fileService = fileService;
        _botClient = botClient;
        _configuration = configuration;
        _mindeeService = mindeeService;
        this.logger = logger;
    }

    public bool CanHandle(Message message) =>
        message != null &&
        message.Type == MessageType.Photo && (SessionStorage.GetSession(message.Chat.Id).Step
        == BotStep.Passport || SessionStorage.GetSession(message!.Chat.Id).Step == BotStep.TechnicalPassport);

    public async Task HandleMessageAsync(Message message, long chatId, CancellationToken cancellationToken)
    {
        var userSession = SessionStorage.GetSession(chatId);

        switch (userSession.Step)
        {
            case BotStep.Passport:

                try
                {
                    await ProcessDocumentAsync(chatId,
                        message,
                        _configuration["DownloadingPaths:PassportFront"]!,
                        _mindeeService.RecognizePassportAsync,
                        PassportMapper.Map,
                        d => userSession.Passport = (PassportModel)d,
                        d => ((PassportModel)d).ToString(),
                        userSession.Step,
                        cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error in DocumentsSubmissionHandler: {ex.Message}\n{ex.StackTrace}");
                    await _botClient.SendMessage(message.Chat.Id, "⚠️ Сталася помилка при обробці документа. Спробуйте ще раз.");
                }

                break;

            case BotStep.TechnicalPassport:

                try
                {
                    await ProcessDocumentAsync(chatId,
                        message,
                        _configuration["DownloadingPaths:TechnicalPassport"]!,
                        _mindeeService.RecognizeTechnicalPassportAsync,
                        TechnicalPassportMapper.Map,
                        d => userSession.TechnicalPassport = (TechnicalPassportModel)d,
                        d => ((TechnicalPassportModel)d).ToString(),
                        userSession.Step,
                        cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error in DocumentsSubmissionHandler: {ex.Message}\n{ex.StackTrace}");
                    await _botClient.SendMessage(message.Chat.Id, "⚠️ Сталася помилка при обробці документа. Спробуйте ще раз.");
                }
                break;
        }
    }

    private async Task ProcessDocumentAsync<TDocument>(
    long chatId,
    Message message,
    string downloadPath,
    Func<string, Task<OperationResultGeneric<TDocument>>> recognizeFunc,
    Func<TDocument, IDocumentData> mapFunc,
    Action<IDocumentData> setData,
    Func<IDocumentData, string> sendData,
    BotStep currentStep,
    CancellationToken cancellationToken)
    {

        var userSession = SessionStorage.GetSession(chatId);
        var downloadingResult = await _fileService.DownloadTgFileAsync(message, _botClient, downloadPath);

        if (!downloadingResult.IsSuccess || downloadingResult.Data == null)
        {
            logger.LogError($"{downloadingResult.ErrorMesage}");
            throw new InvalidOperationException($"Failed to download file: {downloadingResult.ErrorMesage}");
        }
        var recognizeResult = await recognizeFunc.Invoke(downloadingResult.Data);

        if (!recognizeResult.IsSuccess || recognizeResult.Data == null)
        {
            _fileService.DeleteFile(downloadingResult.Data);
            logger.LogError($"Failed to recognize passport: {recognizeResult.ErrorMesage}");
            throw new InvalidOperationException($"Failed to recognize passport: {recognizeResult.ErrorMesage}");
        }

        _fileService.DeleteFile(downloadingResult.Data);

        var document = mapFunc.Invoke(recognizeResult.Data);

        setData.Invoke(document);

        var generatedText = sendData.Invoke(document);

        userSession.Step = UserSession.GetNextStep(currentStep);
        await _botClient.SendMessage(chatId, $"Перевірте, чи всі дані правильні:\n\n\r{generatedText}", replyMarkup: new InlineKeyboardButton[] { "✅ Так", "❌ Ні" }, cancellationToken: cancellationToken);
    }
}