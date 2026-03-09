using Mindee.Parsing.V2.Field;
using TelegramBotConsole;

namespace TelegramBot.Application.Interfaces;

public interface IMindeeService
{
    Task<OperationResultGeneric<InferenceFields>> RecognizePassportAsync(string filePath);
    Task<OperationResultGeneric<InferenceFields>> RecognizeTechnicalPassportAsync(string filePath);
}
