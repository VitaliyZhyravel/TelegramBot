using TelegramBotConsole.User;
using TelegramBotConsole;

namespace TelegramBot.Infrastructure.Interfaces;

public interface IOpenAiService
{
    Task<OperationResultGeneric<string>> GenerateInsuranceAsync(UserSession userSession);
    Task<OperationResultGeneric<string>> GenerateGreetingsAsync();
    Task<OperationResultGeneric<string>> GenerateReplyToUserQuestion(string userMessage);
}
