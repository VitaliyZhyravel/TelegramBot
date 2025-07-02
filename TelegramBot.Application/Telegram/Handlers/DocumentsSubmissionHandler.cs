using Microsoft.Extensions.Configuration;
using Mindee.Parsing.Generated;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Application.Mappings;
using TelegramBot.Domain.Models;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBotConsole;
using TelegramBotConsole.Enums;
using TelegramBotConsole.Models;
using TelegramBotConsole.User;

namespace TelegramBot.Application.Telegram.Handlers;

public class DocumentsSubmissionHandler : IMessageHandler
{
    private readonly Interfaces.IFileService _fileService;
    private readonly ITelegramBotClient _botClient;
    private readonly IConfiguration _configuration;
    private readonly IMindeeService _mindeeService;

    public DocumentsSubmissionHandler(Interfaces.IFileService fileService, ITelegramBotClient botClient, IConfiguration configuration, IMindeeService mindeeService)
    {
        _fileService = fileService;
        _botClient = botClient;
        _configuration = configuration;
        _mindeeService = mindeeService;
    }

    public bool CanHandle(Message message) =>
        message != null &&
        message.Type == MessageType.Photo;

    public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var chatId = message.Chat.Id;
        var userSession = SessionStorage.GetSession(chatId);

        switch (userSession.Step)
        {
            case BotStep.PassportFront:

                try
                {
                    await Handler(
                         message,
                         _configuration["MindeeEndpoints:CustomPassportFront:endpoint"]!,
                         _configuration["MindeeEndpoints:AccountName"]!,
                         _configuration["DownloadingPaths:PassportFront"]!,
                         PassportFrontMapper.Map,
                         data => userSession.PassportFront = (PassportFrontModel)data,
                         data => $"Ім'я (українською): {((PassportFrontModel)data).FullName?.NameUa}\n" +
                                 $"Ім'я (англійською): {((PassportFrontModel)data).FullName?.NameUK}\n" +
                                 $"Дата народження: {((PassportFrontModel)data).DayOfBirth.ToShortDateString()}\n" +
                                 $"Дійсний до: {((PassportFrontModel)data).DateOfExpiry.ToShortDateString()}\n" +
                                 $"Номер запису: {((PassportFrontModel)data).RecordNo}\n" +
                                 $"Номер документа: {((PassportFrontModel)data).DocumentNo}",
                         cancellationToken);
                }
                catch (Exception)
                {
                    throw;
                }

                break;

            case BotStep.PassportBack:
              
                try
                {
                    await Handler(
                        message,
                        _configuration["MindeeEndpoints:CustomPassportBack:endpoint"]!,
                        _configuration["MindeeEndpoints:AccountName"]!,
                        _configuration["DownloadingPaths:PassportBack"]!,
                        CarRegistrationMapper.Map,
                        data => userSession.PassportBack = (PassportBackModel)data,
                        data => $"Identefication: {((PassportBackModel)data).IdentificationCode} \n" +
                                $"Date of Issue: {((PassportBackModel)data).DateOfIssue.ToShortDateString()}\n" +
                                $"Authority : {((PassportBackModel)data).Authority}",
                        cancellationToken);
                }
                catch (Exception)
                {
                    throw;
                }

                break;

            case BotStep.TechnicalPassport:

                try
                {
                    await Handler(
                        message,
                        _configuration["MindeeEndpoints:RegistrationDocument:endpoint"]!,
                        _configuration["MindeeEndpoints:AccountName"]!,
                        _configuration["DownloadingPaths:RegistrationDocument"]!,
                        CarRegistrationMapper.Map,
                        data => userSession.CarRegistration = (CarRegistrationModel)data,
                        data => $"Ім'я (українською): {((CarRegistrationModel)data).FullName?.NameUa}\n" +
                            $"Ім'я (англійською): {((CarRegistrationModel)data).FullName?.NameUK}\n" +
                            $"Реєстраційний номер: {((CarRegistrationModel)data).RegistrationNumber}\n" +
                            $"Дата реєстрації: {((CarRegistrationModel)data).DateOfRegistration!.dateOfRegistration.ToShortDateString()}\n" +
                            $"Дата першої реєстрації: {((CarRegistrationModel)data).DateOfRegistration!.dateOfFirstRegistration.ToShortDateString()}\n" +
                            $"Рік випуску: {((CarRegistrationModel)data).YearOfManufacture}",
                        cancellationToken);
                }
                catch (Exception)
                {
                    throw;
                }
                break;
        }
    }

    public async Task Handler
        (Message message,
        string endpoint,
        string accountName,
        string downloadPath,
        Func<Dictionary<string, GeneratedFeature>, OperationResultGeneric<IDocumentData>> mapper,
        Action<IDocumentData> setUserData,
        Func<IDocumentData, string> generateText,
        CancellationToken cancellationToken)
    {
        var chatId = message.Chat.Id;

        var downloadingResult = await _fileService.DownloadTgFileAsync(message, _botClient, downloadPath);

        if (!downloadingResult.IsSuccess || downloadingResult.Data == null)
        {
            await _botClient.SendMessage(chatId, "Не вдалося отримати фото. Спробуйте ще раз.", cancellationToken: cancellationToken);
            return;
        }
        var recognizeResult = await _mindeeService.RecognizePassportAsync(downloadingResult.Data, endpoint, accountName);

        if (!recognizeResult.IsSuccess || recognizeResult.Data == null)
        {
            await _botClient.SendMessage(chatId, "Не вдалося обробити фото паспорта. Спробуйте ще раз.", cancellationToken: cancellationToken);
            _fileService.DeleteFile(downloadingResult.Data);
            return;
        }

        _fileService.DeleteFile(downloadingResult.Data);

        var mapperResult = mapper.Invoke(recognizeResult.Data);

        if (!mapperResult.IsSuccess || mapperResult.Data == null)
        {
            await _botClient.SendMessage(chatId, "Не вдалося обробити дані паспорта. Спробуйте ще раз.", cancellationToken: cancellationToken);
            return;
        }

        setUserData.Invoke(mapperResult.Data);

        var preview = generateText.Invoke(mapperResult.Data);
        await _botClient.SendMessage(chatId, "Перевірте, чи всі дані правильні:", cancellationToken: cancellationToken);
        await _botClient.SendMessage(chatId, preview, replyMarkup: new InlineKeyboardButton[] { "✅ Так", "❌ Ні" }, cancellationToken: cancellationToken);
    }
}