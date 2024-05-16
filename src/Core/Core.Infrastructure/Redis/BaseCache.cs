using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.Infrastructure.Redis;

public abstract class BaseCache
{
    private readonly IDatabase _dbCache = RedisConnection.Connection.GetDatabase();

    protected T? Get<T>(string key)
    {
        var value= _dbCache.StringGet(key);
        if (value.IsNullOrEmpty)
            return JsonConvert.DeserializeObject<T>(value!);
        return default;
    }

    protected T TryGetAndSet<T>(string key, Func<T> action)
    {
        var result = Get<T>(key);
        if (result is not null) 
            return result;
            
        result = action();
        _dbCache.StringSet(key, JsonConvert.SerializeObject(result));
        return result;
    }

    protected async Task<bool> Remove(string key)
    {
        var existKey = await _dbCache.KeyExistsAsync(key);
        if (!existKey)
            return false;
        return await _dbCache.KeyDeleteAsync(key);
    }
}