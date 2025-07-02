namespace TelegramBotConsole;

public class OperationResult
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMesage { get; private set; }

    public static OperationResult Success() => new OperationResult
    { IsSuccess = true};
    public static OperationResult Failure(string errorMessage) => new OperationResult
    { IsSuccess = false, ErrorMesage = errorMessage };
}