using Mindee.Product.Generated;
using Mindee.Product.InternationalId;
using TelegramBotConsole;

namespace TelegramBot.Infrastructure.Interfaces;

public interface IMindeeService
{
    Task<OperationResultGeneric<InternationalIdV2Document>> RecognizePassportAsync(string filePath);
    Task<OperationResultGeneric<GeneratedV1>> RecognizeTechnicalPassportAsync(string filePath);
}
