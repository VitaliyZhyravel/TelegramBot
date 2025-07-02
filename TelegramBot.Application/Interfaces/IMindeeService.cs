using Mindee.Parsing.Generated;
using TelegramBotConsole;

namespace TelegramBot.Infrastructure.Interfaces;

public  interface IMindeeService
{
    Task<OperationResultGeneric<Dictionary<string, GeneratedFeature>>> RecognizePassportAsync(string filePath,
        string endpointName, string accountName);
}
