using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddSingleton<TelegramBotClient>(new TelegramBotClient(builder.Configuration["TelegramBotToken"]!));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
