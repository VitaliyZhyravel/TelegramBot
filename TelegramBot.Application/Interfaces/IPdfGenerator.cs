namespace TelegramBot.Infrastructure.Interfaces;

public interface IPdfGenerator
{
    void GeneratePdf(string content, string outputPath);
}