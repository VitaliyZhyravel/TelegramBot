namespace TelegramBot.Api.Midleware;

public class ExceptionMidleware
{
    private readonly RequestDelegate _next;
    private readonly Logger<ExceptionMidleware> _logger;

    public ExceptionMidleware(RequestDelegate next, Logger<ExceptionMidleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Internal Server Error");
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
