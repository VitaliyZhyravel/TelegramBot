using Mindee;
using OpenAI.Managers;
using OpenAI;
using Telegram.Bot;
using TelegramBot.Application.Interfaces.Handlers;
using TelegramBot.Application.Telegram.Handlers;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBot.Infrastructure.Services;
using TelegramBotConsole.Services;
using IFileService = TelegramBot.Application.Interfaces.IFileService;

namespace TelegramBot.Api.Dependency_injection_extension
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IMindeeService, MindeeService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IOpenAiService, OpenAiBetalgoiService>();
            builder.Services.AddScoped<IPdfGenerator, PdfGeneratorService>();

            builder.Services.AddScoped<IUpdateHandler, TelegramUpdateHandler>();
            builder.Services.AddScoped<ICallbackHandler, ConfirmDataHandler>();
            builder.Services.AddScoped<ICallbackHandler, ConfirmInsurancePriceHandler>();
            builder.Services.AddScoped<IMessageHandler, DocumentsSubmissionHandler>();
            builder.Services.AddScoped<IMessageHandler, GreetingHandler>();
            builder.Services.AddScoped<IUnknownHandler, UnknownHandler>();

            builder.Services.AddSingleton(new MindeeClient(builder.Configuration["ApiKeys.MindeeService"]));
            builder.Services.AddSingleton(new OpenAIService(new OpenAiOptions { ApiKey = builder.Configuration["ApiKeys.OpenAiService"]! }));
            builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(builder.Configuration["ApiKeys.TelegramBotService"]!));
        }
    }
}
