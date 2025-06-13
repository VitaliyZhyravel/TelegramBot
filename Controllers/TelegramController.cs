using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Api.Controllers
{
    [Route("bot")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly TelegramBotClient botClient;

        public TelegramController(TelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update) 
        {
            if (update.Type == UpdateType.Message && update.Message!.Text != null)
            {
                var chatId = update.Message.Chat.Id;
                var text = update.Message.Text;
                var me = await botClient.GetMe();

                if (text == "/start")
                {
                    await botClient.SendMessage(chatId, $"{me} Привіт! Я допоможу вам з автострахуванням.");
                }
            }

            return Ok();
        }
    }
}
