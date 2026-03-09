namespace TelegramBot.Domain.Enums;

public enum BotStep : byte
{
    Greeting,
    Passport,
    WaitingForConfirmPassport, 
    TechnicalPassport, 
    WaitingForConfirmTechnicalPassport, 
    WaitingForConfirmPrice, 
    Complete
}
