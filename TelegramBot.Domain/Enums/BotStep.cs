namespace TelegramBotConsole.Enums;

public enum BotStep : byte
{
    Greeting = 0,
    Passport = 1,
    WaitingForConfirmPassport = 2,
    TechnicalPassport = 3,
    WaitingForConfirmTechnicalPassport = 4,
    GenerateInsurance = 5
}
