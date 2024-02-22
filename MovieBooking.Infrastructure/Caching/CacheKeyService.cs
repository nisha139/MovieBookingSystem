using MovieBooking.Application.Contracts.Caching;

namespace MovieBooking.InfraStructure.Caching;
public class CacheKeyService : ICacheKeyService
{
    public string GetCacheKey(string name, object id)
    {
        return $"{name}-{id}";
    }
}
