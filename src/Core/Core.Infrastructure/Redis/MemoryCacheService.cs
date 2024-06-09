using Core.Redis;
using Serilog;

namespace Core.Infrastructure.Redis;

public class MemoryCacheService: ICacheService
{
    public MemoryCacheService()
    {
        Log.Information("init cache service using memory cache");       
    }

    public Task<TValue> TryGetThenSet<TValue>(string key, Func<TValue> action)
    {
        return Task.FromResult(action());
    }

    public ValueTask<TValue> Get<TValue>(string key)
    {
        return default;
    }
}