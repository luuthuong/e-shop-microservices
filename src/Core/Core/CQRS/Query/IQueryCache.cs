namespace Core.CQRS.Query;

public interface IQueryCache<out TResponse> : IQuery<TResponse>
{ 
    bool BypassCache { get; }
    string CacheKey { get; }
}