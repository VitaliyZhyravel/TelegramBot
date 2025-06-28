using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using TelegramBot.Infrastructure.Interfaces;

namespace TelegramBotConsole.Services;

public class PdfGeneratorService : IPdfGenerator
{
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
        }
        catch (Exception ex)
        {

            throw;
        }

    }
}
