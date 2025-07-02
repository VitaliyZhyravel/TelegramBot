using TelegramBot.Domain.Models;

namespace TelegramBotConsole.Models;

public class PassportFrontModel : IDocumentData
{
    public FullNames? FullName { get; set; }
    public DateTime DayOfBirth { get; set; }
    public DateTime DateOfExpiry { get; set; }
    public string? RecordNo { get; set; }
    public string? DocumentNo { get; set; }

    public bool IsConfirmed { get; set; }
    public bool IsValid => !string.IsNullOrEmpty(FullName?.NameUa) && !string.IsNullOrEmpty(FullName?.NameUK)
         && DayOfBirth != default && DateOfExpiry != default && !string.IsNullOrEmpty(RecordNo) && !string.IsNullOrEmpty(DocumentNo);
}