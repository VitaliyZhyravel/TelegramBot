using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConsole.Enums
{
    public enum BotStep : byte
    {
        None = 0,
        PassportFront = 2,
        PassportBack = 3,
        TechnicalPassport = 4,
        GenerateInsurance = 5
    }
}
