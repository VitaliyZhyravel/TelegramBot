namespace TelegramBot.Domain.User;

public static class SessionStorage
{
    private static Dictionary<long, UserSession> Sessions { get; set; } = new ();

    public static UserSession GetSession(long userId)
    {
        if (!Sessions.ContainsKey(userId))
        {
            Sessions[userId] = new UserSession();
        }
        return Sessions[userId];
    }
}
