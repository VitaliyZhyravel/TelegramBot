using Mindee.Parsing.Generated;
using TelegramBotConsole;

namespace TelegramBot.Infrastructure.Interfaces;

public  interface IMindeeService
{
    Task<OperationResult<Dictionary<string, GeneratedFeature>>> RecognizePassportAsync(string filePath,
        string endpointName, string accountName);
}
