using Microsoft.Extensions.Logging;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using System.Text;
using TelegramBot.Application.Rules;
using TelegramBot.Infrastructure.Interfaces;
using TelegramBotConsole.Enums;
using TelegramBotConsole.User;

namespace TelegramBotConsole.Services;

public class OpenAiBetalgoiService : IOpenAiService
{
    private readonly OpenAIService _openAi;
    private readonly ILogger<OpenAiBetalgoiService> logger;

    public OpenAiBetalgoiService(OpenAIService openAi, ILogger<OpenAiBetalgoiService> logger)
    {
        _openAi = openAi;
        this.logger = logger;
    }

    public async Task<OperationResultGeneric<string>> GenerateGreetingsAsync()
    {
        var response = await _openAi.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("Ти — Telegram-бот, який вітає користувача і коротко пояснює, що потрібно надіслати фото паспорта. Пиши українською, коротко і дружньо."),

                ChatMessage.FromUser("Згенеруй коротке привітання для Telegram-бота автострахування. Приклад:\r\n\r\n" +
                "👋 Привіт! Я — бот для оформлення автострахування. 📷 Надішліть фото паспорта")
            },
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo
        });

        if (response.Successful)
        {
            var responseMessage = response.Choices.FirstOrDefault()?.Message?.Content;
            if (responseMessage != null)
            {
                logger.LogInformation("Greeting message generated successfully");
                return OperationResultGeneric<string>.Success(responseMessage);
            }
        }

        logger.LogError($"OpenAI response error: {response.Error}");
        return OperationResultGeneric<string>.Failure($"Вибач, щось пішло не так з OpenAI 😞\nError: {response.Error}");
    }

    public async Task<OperationResultGeneric<string>> GenerateInsuranceAsync(UserSession userSession)
    {
        StringBuilder sb = new StringBuilder();

        var response = await _openAi.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("Ти — помічник, який генерує приклади текстів страхових полісів для навчальних або демонстраційних проектів. " +
                "Твоє завдання — створити фіктивний текст автостраховки  по шаблону наданому нижче."),
                ChatMessage.FromUser(RuleForGpt.RuleForGenerateInsuranse(userSession))

            },
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo
        });

        if (response.Successful)
        {
            var responseMessage = response.Choices.FirstOrDefault()?.Message?.Content;

            if (responseMessage != null)
            {
                logger.LogInformation("Insurance text generated successfully");
                return OperationResultGeneric<string>.Success(responseMessage);
            }
        }

        logger.LogError($"OpenAI response error: {response.Error}");
        return OperationResultGeneric<string>.Failure($"Вибач, щось пішло не так з OpenAI 😞\nError: {response.Error}");
    }

    public async Task<OperationResultGeneric<string>> GenerateReplyToUserQuestion(string userMessage) 
    {

        var response = await _openAi.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(RuleForGpt.RuleForNotHandleUserMessage()),
                ChatMessage.FromUser($"Користувач запитує: {userMessage}")
            },
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo
        });

        if (response.Successful)
        {
            var responseMessage = response.Choices.FirstOrDefault()?.Message?.Content;
            if (responseMessage != null)
            {
                logger.LogInformation("Reply on user question generated successfully");
                return OperationResultGeneric<string>.Success(responseMessage);
            }
        }

        logger.LogError($"OpenAI response error: {response.Error}");
        return OperationResultGeneric<string>.Failure($"Вибач, щось пішло не так з OpenAI 😞\nError: {response.Error}");
    }
}
