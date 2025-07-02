namespace TelegramBotConsole;

public class OperationResultGeneric<TData>
{
    public bool IsSuccess { get; private set; }
    public TData? Data { get; private set; }
    public string? ErrorMesage { get; private set; }

    public static OperationResultGeneric<TData> Success(TData data) => new OperationResultGeneric<TData>
    { IsSuccess = true, Data = data };
    public static  OperationResultGeneric<TData> Failure(string errorMessage) => new OperationResultGeneric<TData>
    { IsSuccess = false, ErrorMesage = errorMessage };
}
