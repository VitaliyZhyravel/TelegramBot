namespace TelegramBot.Application.Interfaces;

public interface IPdfGenerator
{
    void GeneratePdf(string content, string outputPath);
}