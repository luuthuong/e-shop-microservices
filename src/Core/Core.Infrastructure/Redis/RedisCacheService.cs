using Core.Redis;
using Serilog;

namespace Core.Infrastructure.Redis;

public class RedisCacheService: ICacheService
{
    public RedisCacheService()
    {
        Log.Information("init cache service using redis");       
    }

    public Task<TValue> TryGetThenSet<TValue>(string key, Func<TValue> action)
    {
        return Task.FromResult(action());
    }

    public ValueTask<TValue> Get<TValue>(string key)
    {
        throw new NotImplementedException();
    }
}