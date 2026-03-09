using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Microsoft.Extensions.Logging;
using TelegramBot.Application.Interfaces;
using TelegramBot.Application.Rules;
using TelegramBot.Domain.User;
using TelegramBotConsole;

namespace TelegramBot.Infrastructure.Services;

public class OpenAiBetalgoiService : IOpenAiService
{
    private readonly OpenAIService _openAi;
    private readonly ILogger<OpenAiBetalgoiService> _logger;

    public OpenAiBetalgoiService(
        OpenAIService openAi,
        ILogger<OpenAiBetalgoiService> logger)
    {
        _openAi = openAi;
        _logger = logger;
    }

    public async Task<OperationResultGeneric<string>> GenerateInsuranceAsync(UserSession userSession)
    {
        try
        {
            var response = await _openAi.ChatCompletion.CreateCompletion(
                new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem(
                            "Ти — помічник, який генерує приклади текстів страхових полісів для навчальних або демонстраційних проектів. " +
                            "Твоє завдання — створити фіктивний текст автостраховки по шаблону, наданому нижче."
                        ),
                        ChatMessage.FromUser(RulesForGpt.RuleForGenerateInsuranse(userSession))
                    },
                    Model = "gpt-4o"
                });

            if (response.Successful)
            {
                var responseMessage = response.Choices.FirstOrDefault()?.Message.Content;

                if (!string.IsNullOrWhiteSpace(responseMessage))
                {
                    _logger.LogInformation("Insurance text generated successfully");
                    return OperationResultGeneric<string>.Success(responseMessage);
                }
            }

            _logger.LogError(
                "Class: {ClassName}\nMethod: {MethodName}\nInsurance text was not generated.\nError: {Error}",
                nameof(OpenAiBetalgoiService),
                nameof(GenerateInsuranceAsync),
                response.Error?.Message ?? response.Error?.ToString());

            return OperationResultGeneric<string>.Failure(
                response.Error?.Message ?? "OpenAI request failed");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Class: {ClassName}\nMethod: {MethodName}\nUnexpected error while generating insurance text",
                nameof(OpenAiBetalgoiService),
                nameof(GenerateInsuranceAsync));

            return OperationResultGeneric<string>.Failure(
                $"Unexpected error: {ex.Message}");
        }
    }

    public async Task<OperationResultGeneric<string>> GenerateReplyToUserQuestion(string userMessage)
    {
        try
        {
            var response = await _openAi.ChatCompletion.CreateCompletion(
                new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem(RulesForGpt.RuleForNotHandleUserMessage()),
                        ChatMessage.FromUser($"Користувач запитує: {userMessage}")
                    },
                    Model = "gpt-4o"
                });

            if (response.Successful)
            {
                var responseMessage = response.Choices.FirstOrDefault()?.Message.Content;

                if (!string.IsNullOrWhiteSpace(responseMessage))
                {
                    _logger.LogInformation("Reply on user question generated successfully");
                    return OperationResultGeneric<string>.Success(responseMessage);
                }
            }

            _logger.LogError(
                "Class: {ClassName}\nMethod: {MethodName}\nReply was not generated.\nError: {Error}",
                nameof(OpenAiBetalgoiService),
                nameof(GenerateReplyToUserQuestion),
                response.Error?.Message ?? response.Error?.ToString());

            return OperationResultGeneric<string>.Failure(
                response.Error?.Message ?? "OpenAI request failed");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Class: {ClassName}\nMethod: {MethodName}\nUnexpected error while generating reply",
                nameof(OpenAiBetalgoiService),
                nameof(GenerateReplyToUserQuestion));

            return OperationResultGeneric<string>.Failure(
                $"Unexpected error: {ex.Message}");
        }
    }
}