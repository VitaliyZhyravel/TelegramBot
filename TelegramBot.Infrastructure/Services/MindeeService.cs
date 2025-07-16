using Microsoft.Extensions.Logging;
using Mindee;
using Mindee.Http;
using Mindee.Input;
using Mindee.Product.Generated;
using Mindee.Product.InternationalId;
using TelegramBot.Infrastructure.Interfaces;

namespace TelegramBotConsole.Services;

public class MindeeService : IMindeeService
{
    private readonly MindeeClient _client;
    private readonly ILogger<MindeeService> logger;

    public MindeeService(MindeeClient client, ILogger<MindeeService> logger)
    {
        _client = client;
        this.logger = logger;
    }

    public async Task<OperationResultGeneric<InternationalIdV2Document>> RecognizePassportAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                logger.LogError($"Class: {nameof(MindeeService)}\nMethod: {nameof(RecognizePassportAsync)}\nError: File not found");
                return OperationResultGeneric<InternationalIdV2Document>.Failure("File not found");
            }

            var inputSource = new LocalInputSource(filePath);

            var apiResponse = await _client.EnqueueAndParseAsync<InternationalIdV2>(inputSource);

            var generatedFeatures = apiResponse.Document.Inference.Prediction;

            if (generatedFeatures == null)
            {
                logger.LogError($"Class: {nameof(MindeeService)}\nMethod: {nameof(RecognizePassportAsync)}\nError: Data converted from photo is empty");
                return OperationResultGeneric<InternationalIdV2Document>.Failure("Data converted from photos is empty");
            }

            logger.LogInformation("Passport recognized successfully");
            return OperationResultGeneric<InternationalIdV2Document>.Success(generatedFeatures);
        }
        catch (Exception ex)
        {
            logger.LogError($"Class: {nameof(MindeeService)}\nMethod: {nameof(RecognizePassportAsync)}\nError: Document processing error {ex.Message}");
            return OperationResultGeneric<InternationalIdV2Document>.Failure($"Document processing\nError: {ex.Message}");
        }
    }

    public async Task<OperationResultGeneric<GeneratedV1>> RecognizeTechnicalPassportAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                logger.LogError($"Class: {nameof(MindeeService)}\nMethod: {nameof(RecognizeTechnicalPassportAsync)}\nError: File not found");
                return OperationResultGeneric<GeneratedV1>.Failure("File not found");
            }

            var inputSource = new LocalInputSource(filePath);

            CustomEndpoint endpoint = new CustomEndpoint(
                endpointName: "technicalpassport",
                accountName: "VitaliyZhyravel",
                version: "1"
                );

            var apiResponse = await _client.EnqueueAndParseAsync<GeneratedV1>(inputSource,endpoint);

            var generatedFeatures = apiResponse.Document.Inference;

            if (generatedFeatures.Prediction.Fields == null)
            {
                logger.LogError($"Class: {nameof(MindeeService)}\nMethod: {nameof(RecognizeTechnicalPassportAsync)}\nError: Data converted from photo is empty");
                return OperationResultGeneric<GeneratedV1>.Failure("Data converted from photos is empty");
            }

            logger.LogInformation("Technical passport recognized successfully");
            return OperationResultGeneric<GeneratedV1>.Success(generatedFeatures);
        }
        catch (Exception ex)
        {
            logger.LogError($"Class: {nameof(MindeeService)}\nMethod: {nameof(RecognizeTechnicalPassportAsync)}\nError: Document processing\nError: {ex.Message}");
            return OperationResultGeneric<GeneratedV1>.Failure($"{ex.Message}");
        }
    }
}