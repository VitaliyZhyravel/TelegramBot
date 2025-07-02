using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConsole.User
{
    public class SessionStorage
    {
        private static Dictionary<long, UserSession> Sessions { get; set; } = new Dictionary<long, UserSession>();

        public static UserSession GetSession(long userId)
        {
            if (!Sessions.ContainsKey(userId))
            {
                Sessions[userId] = new UserSession();
            }
            return Sessions[userId];
        }
    }
}
