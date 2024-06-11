using System.Text;
using Core.Redis;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Core.Infrastructure.Redis;

public class MemoryCacheService(MemoryCache memoryCache) : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(memoryCache.TryGetValue(key, out T? cachedData) ? cachedData : default);
    }

    public Task<bool> SetAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        var cacheEntryOptions  = new MemoryCacheEntryOptions();
        var action = Task.FromResult(memoryCache.Set<T>(key, data, cacheEntryOptions)).IsCompletedSuccessfully;
        return Task.FromResult(action);
    }

    public async Task<T?> TryGetAndSet<T>(string key, Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        if (memoryCache.TryGetValue(key, out T? cachedDataValue))
        {
            return cachedDataValue;
        }

        var response = await action();
        var cacheEntryOptions  = new MemoryCacheEntryOptions();
        memoryCache.Set(key, response, cacheEntryOptions);
        return response;
    }

    public void RemoveAsync(string key)
    {
        memoryCache.Remove(key);
    }
}