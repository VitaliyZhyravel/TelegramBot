namespace TelegramBotConsole.Models;

public class CarRegistrationModel
{
    public FullNames? FullName { get; set; }
    public string? RegistrationNumber { get; set; }
    public DatesOfRegistration? DateOfRegistration { get; set; }
    public string? YearOfManufacture { get; set; }

    public bool IsValid => !string.IsNullOrEmpty(FullName?.NameUa) && !string.IsNullOrEmpty(FullName?.NameUK)
        && RegistrationNumber != default && DateOfRegistration?.dateOfFirstRegistration != default 
        && DateOfRegistration?.dateOfRegistration != default && YearOfManufacture != default;
}
