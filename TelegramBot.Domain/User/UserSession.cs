using TelegramBot.Application.Mappings;
using TelegramBotConsole.Enums;

namespace TelegramBotConsole.User;

public class UserSession
{
    public BotStep Step { get; set; } = BotStep.Greeting;

    public static BotStep GetNextStep(BotStep currentStep)
    {
        var values = Enum.GetValues(typeof(BotStep)).Cast<BotStep>().ToList();
        int index = values.IndexOf(currentStep);

        return (index >= 0 && index < values.Count - 1)
            ? values[index + 1]
            : currentStep; 
    }

    public static BotStep GetPreviousStep(BotStep currentStep)
    {
        var values = Enum.GetValues(typeof(BotStep)).Cast<BotStep>().ToList();
        int index = values.IndexOf(currentStep);

        return (index >= 1 && index < values.Count - 1)
            ? values[index - 1]
            : currentStep;
    }

    public PassportModel? Passport { get; set; }
    public TechnicalPassportModel? TechnicalPassport { get; set; }
}
