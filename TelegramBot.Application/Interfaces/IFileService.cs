using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBotConsole;

namespace TelegramBot.Application.Interfaces;

public interface IFileService
{
    Task<OperationResultGeneric<string?>> DownloadTgFileAsync(Message message, ITelegramBotClient bot, string path);
    void DeleteFile(string filePath);
}
