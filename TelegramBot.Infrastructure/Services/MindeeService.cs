using Mindee;
using Mindee.Http;
using Mindee.Input;
using Mindee.Parsing.Generated;
using Mindee.Product.Generated;
using TelegramBot.Infrastructure.Interfaces;

namespace  TelegramBotConsole.Services;

public class MindeeService : IMindeeService
{
    private readonly MindeeClient _client;

    public MindeeService(MindeeClient client)
    {
        _client = client;
    }

    public async Task<OperationResult<Dictionary<string, GeneratedFeature>>> RecognizePassportAsync(string filePath,
        string endpointName, string accountName)
    {
        try
        {
            var endpoint = new CustomEndpoint(endpointName, accountName);
            var inputSource = new LocalInputSource(filePath);

            var apiResponse = await _client.EnqueueAndParseAsync<GeneratedV1>(inputSource, endpoint);

            var generatedFeatures = apiResponse.Document.Inference.Prediction.Fields;

            if (generatedFeatures == null || generatedFeatures.Count == 0)
            {
                return OperationResult<Dictionary<string, GeneratedFeature>>.Failure("No features found in the document.");
            }
            return OperationResult<Dictionary<string, GeneratedFeature>>.Sucssecc(generatedFeatures);
        }
        catch (Exception ex)
        {
            return OperationResult<Dictionary<string, GeneratedFeature>>.Failure($"Error processing document: {ex.Message}");
        }
    }
}