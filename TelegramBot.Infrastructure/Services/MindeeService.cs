using Microsoft.Extensions.Logging;
using Mindee;
using Mindee.Input;
using Mindee.Parsing.V2.Field;
using TelegramBot.Application.Interfaces;
using TelegramBotConsole;

namespace TelegramBot.Infrastructure.Services;

public class MindeeService : IMindeeService
{
    private readonly MindeeClientV2 _clientV2;
    private readonly ILogger<MindeeService> _logger;
    private const string PassportModelId = "30bdc2eb-45da-4522-8284-15d5ab533dd2";
    private const string TechnicalPassportModelId = "a5f69b32-777e-436e-bb0d-4f51b52471c6"; 

    public MindeeService(ILogger<MindeeService> logger, MindeeClientV2 clientV2)
    {
        _logger = logger;
        _clientV2 = clientV2;
    }

    public async Task<OperationResultGeneric<InferenceFields>> RecognizePassportAsync(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _logger.LogError(
                    "Class: {ClassName}\nMethod: {MethodName}\nError: File path is empty",
                    nameof(MindeeService),
                    nameof(RecognizePassportAsync));

                return OperationResultGeneric<InferenceFields>.Failure("File path is empty");
            }

            if (!File.Exists(filePath))
            {
                _logger.LogError(
                    "Class: {ClassName}\nMethod: {MethodName}\nError: File not found",
                    nameof(MindeeService),
                    nameof(RecognizePassportAsync));

                return OperationResultGeneric<InferenceFields>.Failure("File not found");
            }

            var inputSource = new LocalInputSource(filePath);

            var inferenceParams = new InferenceParameters(
                modelId: PassportModelId,
                rag: null,
                rawText: null,
                polygon: null,
                confidence: null
            );

            var response = await _clientV2.EnqueueAndGetInferenceAsync(inputSource, inferenceParams);

            var fields = response?.Inference?.Result?.Fields;

            if (fields == null)
            {
                _logger.LogError(
                    "Class: {ClassName}\nMethod: {MethodName}\nError: Data converted from photo is empty",
                    nameof(MindeeService),
                    nameof(RecognizePassportAsync));

                return OperationResultGeneric<InferenceFields>.Failure("Data converted from photo is empty");
            }

            _logger.LogInformation(
                "Class: {ClassName}\nMethod: {MethodName}\nMessage: Passport recognized successfully",
                nameof(MindeeService),
                nameof(RecognizePassportAsync));

            return OperationResultGeneric<InferenceFields>.Success(fields);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Class: {ClassName}\nMethod: {MethodName}\nError: Document processing error",
                nameof(MindeeService),
                nameof(RecognizePassportAsync));

            return OperationResultGeneric<InferenceFields>.Failure($"Document processing error: {ex.Message}");
        }
    }

    public async Task<OperationResultGeneric<InferenceFields>> RecognizeTechnicalPassportAsync(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _logger.LogError(
                    "Class: {ClassName}\nMethod: {MethodName}\nError: File path is empty",
                    nameof(MindeeService),
                    nameof(RecognizeTechnicalPassportAsync));

                return OperationResultGeneric<InferenceFields>.Failure("File path is empty");
            }

            if (!File.Exists(filePath))
            {
                _logger.LogError(
                    "Class: {ClassName}\nMethod: {MethodName}\nError: File not found",
                    nameof(MindeeService),
                    nameof(RecognizeTechnicalPassportAsync));

                return OperationResultGeneric<InferenceFields>.Failure("File not found");
            }

            var inputSource = new LocalInputSource(filePath);

            var inferenceParams = new InferenceParameters(
                modelId: TechnicalPassportModelId,
                rag: null,
                rawText: null,
                polygon: null,
                confidence: null
            );

            var response = await _clientV2.EnqueueAndGetInferenceAsync(inputSource, inferenceParams);

            var fields = response?.Inference?.Result?.Fields;

            if (fields == null)
            {
                _logger.LogError(
                    "Class: {ClassName}\nMethod: {MethodName}\nError: Data converted from photo is empty",
                    nameof(MindeeService),
                    nameof(RecognizeTechnicalPassportAsync));

                return OperationResultGeneric<InferenceFields>.Failure("Data converted from photo is empty");
            }

            _logger.LogInformation(
                "Class: {ClassName}\nMethod: {MethodName}\nMessage: Technical passport recognized successfully",
                nameof(MindeeService),
                nameof(RecognizeTechnicalPassportAsync));

            return OperationResultGeneric<InferenceFields>.Success(fields);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Class: {ClassName}\nMethod: {MethodName}\nError: Document processing error",
                nameof(MindeeService),
                nameof(RecognizeTechnicalPassportAsync));

            return OperationResultGeneric<InferenceFields>.Failure($"Document processing error: {ex.Message}");
        }
    }
}