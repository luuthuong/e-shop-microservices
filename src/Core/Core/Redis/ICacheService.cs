namespace Core;

public interface ICacheService
{
    T Get<T>(string key);

    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task<bool> SetAsync<T>(string key, T data);

    T TryGetAndSet<T>(string key, Func<T> action);

    Task<T> TryGetAndSet<T>(string key, Func<Task<T>> action, CancellationToken cancellationToken = default);

    Task<bool> RemoveAsync(string key);
}
