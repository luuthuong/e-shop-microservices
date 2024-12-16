using Core.CQRS.Query;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CacheSettings = Core.CQRS.Query.CacheSettings;
using Core.Redis;

namespace Core.Infrastructure.CQRS;

public class CachedBehavior<TRequest, TResponse>(
        ILogger<TResponse> logger, 
        IOptions<CacheSettings> settings,
        ICacheService cacheService
) : IPipelineBehavior<TRequest, TResponse> where TRequest: notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        switch (request)
        {
            case IClearCache cacheRequest:
                cacheService.RemoveAsync(cacheRequest.Key);
                return await next();
            case IQueryCache<TResponse> { BypassCache: true }:
                return await next();
            default:
            {
                if (request is IQueryCache<TResponse> queryCache)
                    await cacheService.TryGetAndSet<TResponse>(queryCache.CacheKey, next.Invoke,cancellationToken);
                return await next();
            }
        }
    }
}