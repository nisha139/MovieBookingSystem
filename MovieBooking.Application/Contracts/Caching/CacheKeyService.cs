using MovieBooking.Application.Interfaces;

namespace MovieBooking.Application.Contracts.Caching;
public interface ICacheKeyService : IScopedService
{
    public string GetCacheKey(string name, object id);
}
