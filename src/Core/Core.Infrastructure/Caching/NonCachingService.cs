using Core.Redis;

namespace Core.Infrastructure.Caching;

public class NonCachingService : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) => Task.FromResult<T?>(default);

    public void RemoveAsync(string key) {}

    public Task<bool> SetAsync<T>(string key, T data, CancellationToken cancellationToken = default) => Task.FromResult(true);

    public Task<T?> TryGetAndSet<T>(string key, Func<Task<T>> action, CancellationToken cancellationToken = default) => action()!;
}
