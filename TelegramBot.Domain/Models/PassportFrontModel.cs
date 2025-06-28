namespace TelegramBotConsole.Models;

public partial class PassportFrontModel
{
    public FullNames? FullName { get; set; }
    public DateTime DayOfBirth { get; set; }
    public DateTime DateOfExpiry { get; set; }
    public string? RecordNo { get; set; }
    public string? DocumentNo { get; set; }

    public bool IsValid => !string.IsNullOrEmpty(FullName?.NameUa) && !string.IsNullOrEmpty(FullName?.NameUK)
         && DayOfBirth != default && DateOfExpiry != default && !string.IsNullOrEmpty(RecordNo) && !string.IsNullOrEmpty(DocumentNo);
}