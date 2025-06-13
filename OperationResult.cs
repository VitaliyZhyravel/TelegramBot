namespace TelegramBot.Api
{
    public class OperationResult<TData>
    {
        public bool IsSuccess { get; set; }

        private TData? Data { get; set; }

        private string? ErrorMessage { get; set; }

        public static OperationResult<TData> Success(TData data) => new OperationResult<TData> { IsSuccess = true, Data = data };
        public static OperationResult<TData> Failure(string errorMessage) => new OperationResult<TData> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
