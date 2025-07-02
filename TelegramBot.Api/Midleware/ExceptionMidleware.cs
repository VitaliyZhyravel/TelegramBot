namespace TelegramBot.Api.Midleware;

public class ExceptionMidleware
{
    private readonly RequestDelegate _next;

    public ExceptionMidleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}

public static class ExceptionMidlewareExtensions
{
    public static IApplicationBuilder UseExceptionMidleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMidleware>();
    }
}
