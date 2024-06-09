using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Core.Redis;

public class CacheService(
    IDistributedCache cache
) : ICacheService
{

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedResponse = await cache.GetAsync(key, cancellationToken);
        return cachedResponse is not null ? JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(cachedResponse)) : default;
    }

    public async Task<bool> SetAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions { };
        var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(data));
        await cache.SetAsync(key, serializedData, options, cancellationToken);
        return true;
    }

    public async Task<T?> TryGetAndSet<T>(string key, Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        var cachedResponse = await cache.GetAsync(key, cancellationToken);

        if(cachedResponse is not null)
            return JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(cachedResponse));
        var data = await action();     
        var options = new DistributedCacheEntryOptions{};
        var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(data));
        await cache.SetAsync(key, serializedData, options, cancellationToken);
        return data;
    }

    public void RemoveAsync(string key)
    {
        cache.Remove(key);
    }
    
}