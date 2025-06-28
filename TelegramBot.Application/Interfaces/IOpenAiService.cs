using TelegramBotConsole.User;
using TelegramBotConsole;

namespace TelegramBot.Infrastructure.Interfaces;

public interface IOpenAiService
{
    Task<OperationResult<string>> GenerateInsurance(UserSession userSession);
}
