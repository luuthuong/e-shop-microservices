namespace Core.Redis;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task<bool> SetAsync<T>(string key, T data, CancellationToken cancellationToken = default);

    Task<T?> TryGetAndSet<T>(string key, Func<Task<T>> action, CancellationToken cancellationToken = default);

    void RemoveAsync(string key);
}

