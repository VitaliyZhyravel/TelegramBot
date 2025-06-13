using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.AddUserSecrets<Program>();

var token = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN");
builder.Services.AddSingleton<TelegramBotClient>(new TelegramBotClient(token!));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
