using Core.CQRS.Query;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Redis;

namespace Core.Infrastructure.CQRS;

public class CacheRequestBehavior<TRequest, TResponse>(
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
            {
                logger.LogDebug("Bypassing cache for request: {name} {@request}", typeof(TRequest).Name, request);
                return await next();
            }
            
            default:
            {
                if (request is IQueryCache<TResponse> queryCache)
                {
                    await cacheService.TryGetAndSet<TResponse>(queryCache.CacheKey, next.Invoke,cancellationToken);
                }
                return await next();
            }
        }
    }
}