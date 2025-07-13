using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Application.Interfaces;
using TelegramBotConsole;

namespace TelegramBot.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly ILogger<FileService> logger;

    public FileService(ILogger<FileService> logger)
    {
        this.logger = logger;
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath)) File.Delete(filePath);
    }

    public async Task<OperationResultGeneric<string?>> DownloadTgFileAsync(Message message, ITelegramBotClient bot, string path)
    {
        if (message?.Photo != null)
        {
            var fileId = message.Photo[^1].FileId;
            var tgFile = await bot.GetFile(fileId);

            var result = ValidTgFile(tgFile);

            if (!result.IsSuccess)
            {
                logger.LogError($"Class: {nameof(FileService)}\nMethod: {nameof(DownloadTgFileAsync)}\nError: {result.ErrorMesage}");
                return OperationResultGeneric<string?>.Failure(result.ErrorMesage!);
            }

            var filePathFromTelegram = tgFile.FilePath;

            var fileName = $"{tgFile.FileUniqueId}{Path.GetExtension(filePathFromTelegram)}";
            var fullPath = Path.Combine(path, fileName);

            if (filePathFromTelegram != null)
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    await bot.DownloadFile(filePathFromTelegram, stream);
                }

                logger.LogInformation($"File downloaded successfully");
                return OperationResultGeneric<string?>.Success(fullPath);
            }
        }
        logger.LogError($"Class: {nameof(FileService)}\nMethod: {nameof(DownloadTgFileAsync)}\nError: No photo found in the message");
        return OperationResultGeneric<string?>.Failure("No photo found in the message");
    }

    private OperationResultGeneric<string> ValidTgFile(TGFile tgFile)
    {
        List<string> avaiableExtensions = new List<string> { ".jpg", ".jpeg", ".png" };

        var tgFilePath = tgFile.FilePath;

        if (tgFile.FilePath != null)
        {
            var fileExtension = Path.GetExtension(tgFilePath);

            var isValidExtension = avaiableExtensions.Any(x => x == fileExtension);

            if (isValidExtension)
            {
                if (tgFile.FileSize > 0 && tgFile.FileSize < 5 * 1024 * 1024)
                {
                    return OperationResultGeneric<string>.Success(string.Empty);
                }
                else
                {
                    return OperationResultGeneric<string>.Failure("The file size must not exceed 5 MB");
                }
            }
            else
            {
                return OperationResultGeneric<string>.Failure("Unsupported file format. Please send images in JPG, JPEG, or PNG format");
            }
        }

        return OperationResultGeneric<string>.Failure("Download error");
    }
}