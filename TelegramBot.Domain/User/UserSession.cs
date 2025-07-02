using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConsole.Enums;
using TelegramBotConsole.Models;

namespace TelegramBotConsole.User
{
    public class UserSession
    {
        public BotStep Step { get; set; }

        public PassportFrontModel? PassportFront { get; set; }
        public PassportBackModel? PassportBack { get; set; }
        public CarRegistrationModel? CarRegistration { get; set; }
    }
}
