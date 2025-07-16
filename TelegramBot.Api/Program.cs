using TelegramBot.Api.Dependency_injection_extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.ConfigureServices(builder);

var app = builder.Build();

app.MapGet("/", () => "Telegram Bot API is running!");

app.MapControllers();

app.Run();
