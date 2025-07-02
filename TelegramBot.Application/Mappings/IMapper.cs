using Mindee.Parsing.Generated;
using TelegramBotConsole;

namespace TelegramBot.Application.Mappings;

internal interface IMapper<TOut>
{
    OperationResultGeneric<TOut> Map(Dictionary<string, GeneratedFeature> dataFromFile);
}
