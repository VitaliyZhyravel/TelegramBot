using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using IUpdateHandler = TelegramBot.Application.Interfaces.Handlers.IUpdateHandler;

namespace TelegramBot.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BotController : ControllerBase
{
    private readonly IUpdateHandler updateHandler;
    private readonly ITelegramBotClient telegramBot;

    public BotController(IUpdateHandler updateHandler,ITelegramBotClient telegramBot)
    {
        this.updateHandler = updateHandler;
        this.telegramBot = telegramBot;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        try
        {
            await updateHandler.HandleUpdateAsync(update, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
