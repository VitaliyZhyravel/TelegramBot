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
    private readonly ILogger<BotController> logger;

    public BotController(IUpdateHandler updateHandler, ITelegramBotClient telegramBot,ILogger<BotController> logger)
    {
        this.updateHandler = updateHandler;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        try
        {
            await updateHandler.HandleUpdateAsync(update, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while processing the update. Error : {ex.Message}");
        }
        return Ok();
    }
}
