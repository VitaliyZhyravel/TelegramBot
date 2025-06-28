namespace TelegramBotConsole;

public class OperationResult<TData>
{
    public bool IsSuccess { get; private set; }
    public TData? Data { get; private set; }
    public string? ErrorMesage { get; private set; }

    public static OperationResult<TData> Sucssecc(TData data) => new OperationResult<TData>
    { IsSuccess = true, Data = data };
    public static  OperationResult<TData> Failure(string errorMessage) => new OperationResult<TData>
    { IsSuccess = false, ErrorMesage = errorMessage };
}
