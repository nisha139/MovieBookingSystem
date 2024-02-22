namespace MovieBooking.InfraStructure.Caching;

public class CacheKeyConstants
{
    public static string NotifyUser(string userId) => $"notify-user-{userId}";
}
