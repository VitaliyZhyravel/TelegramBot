using Microsoft.Extensions.Logging;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using TelegramBot.Infrastructure.Interfaces;

namespace TelegramBotConsole.Services;

public class PdfGeneratorService : IPdfGenerator
{
    private readonly ILogger<PdfGeneratorService> logger;

    public PdfGeneratorService(ILogger<PdfGeneratorService> logger)
    {
        this.logger = logger;
    }

    public void GeneratePdf(string content, string outputPath)
    {
        try
        {
            Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Content()
                        .Padding(20)
                        .Text(content);
                });
            })
            .GeneratePdf(outputPath);
            logger.LogInformation($"PDF generated successfully");
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Class: {nameof(PdfGeneratorService)} Method: {nameof(GeneratePdf)}\nError generating PDF\nError: {ex.Message}");
            throw;
        }
    }
}
