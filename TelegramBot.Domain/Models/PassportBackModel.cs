namespace TelegramBotConsole.Models;

public class PassportBackModel
{
    public string? IdentificationCode { get; set; }
    public DateTime DateOfIssue { get; set; }
    public string? Authority { get; set; }

    public bool IsValid =>  DateOfIssue != default && IdentificationCode != default && Authority != default;
}
