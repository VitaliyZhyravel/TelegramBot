using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBotConsole;

namespace TelegramBot.Application.Interfaces;

public interface IFileService
{
    Task<OperationResult<string?>> DownloadTgFile(Message message, TelegramBotClient bot, string path);
}
