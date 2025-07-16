using Microsoft.Extensions.Logging;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using System.Text;
using TelegramBot.Application.Rules;
using TelegramBot.Infrastructure.Interfaces;
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
            Model = OpenAI.ObjectModels.Models.Chatgpt_4o_latest
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

        logger.LogError($"Class: {nameof(OpenAiBetalgoiService)} Method: {nameof(GenerateReplyToUserQuestion)}\nInsuranse text not create successfully\nError: {response.Error}");
        return OperationResultGeneric<string>.Failure($"Error: {response.Error}");
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
            Model = OpenAI.ObjectModels.Models.Chatgpt_4o_latest,
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

        logger.LogError($"Class: {nameof(OpenAiBetalgoiService)} Method: {nameof(GenerateReplyToUserQuestion)}\nReply on user question not generated successfully\nError: {response.Error}");
        return OperationResultGeneric<string>.Failure($"{response.Error}");
    }
}
