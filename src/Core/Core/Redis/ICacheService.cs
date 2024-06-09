namespace Core.Redis;

public interface ICacheService
{
    Task<TValue> TryGetThenSet<TValue>(string key, Func<TValue> action);

    public ValueTask<TValue> Get<TValue>(string key);
}