using Mindee;
using OpenAI;
using OpenAI.Managers;
using Telegram.Bot;
using TelegramBot.Api.Midleware;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Application.Telegram.Handlers;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBot.Infrastructure.Services;
using TelegramBotConsole.Services;
using IFileService = TelegramBot.Application.Interfaces.IFileService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IMindeeService, MindeeService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IOpenAiService, OpenAiBetalgoiService>();
builder.Services.AddScoped<IPdfGenerator, PdfGeneratorService>();

builder.Services.AddScoped<IUpdateHandler, TelegramUpdateHandler>();
builder.Services.AddScoped<ICallbackHandler, ConfirmDataHandler>();
builder.Services.AddScoped<ICallbackHandler, ConfirmInsurancePriceHandler>();
builder.Services.AddScoped<IMessageHandler, DocumentSelectionHandler>();
builder.Services.AddScoped<IMessageHandler, DocumentsSubmissionHandler>();
builder.Services.AddScoped<IMessageHandler, GreetingsHandler>();
builder.Services.AddScoped<IUnknownHandler, UnknownHandler>();

builder.Services.AddSingleton<MindeeClient>(new MindeeClient(builder.Configuration["ApiKeys.MindeeService"]));
builder.Services.AddSingleton<OpenAIService>(new OpenAIService(new OpenAiOptions { ApiKey = builder.Configuration["ApiKeys.OpenAiService"]! }));
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(builder.Configuration["ApiKeys.TelegramBotService"]!));

var app = builder.Build();

if (builder.Environment.IsProduction())
{
    app.UseExceptionMidleware();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Telegram Bot API is running!");

app.MapControllers();

app.Run();
