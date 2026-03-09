using TelegramBot.Domain.User;
using TelegramBotConsole;

namespace TelegramBot.Application.Interfaces;

public interface IOpenAiService
{
    Task<OperationResultGeneric<string>> GenerateReplyToUserQuestion(string userMessage);
}
