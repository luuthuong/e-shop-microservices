using Core.Redis;

namespace Core.Infrastructure.Caching;

public class MemoryCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<T?> TryGetAndSet<T>(string key, Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void RemoveAsync(string key)
    {
        throw new NotImplementedException();
    }
}