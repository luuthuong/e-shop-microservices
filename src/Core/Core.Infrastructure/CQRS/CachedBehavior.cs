using System.Text;
using Core.CQRS.Query;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CacheSettings = Core.CQRS.Query.CacheSettings;
using Core.Redis;

namespace Core.Infrastructure.CQRS;

public class CachedBehavior<TRequest, TResponse>(
        IDistributedCache cache, 
        ILogger<TResponse> logger, 
        IOptions<CacheSettings> settings,
        ICacheService cacheService
) : IPipelineBehavior<TRequest, TResponse> where TRequest: notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        if(request is IClearCache) {
            cacheService.RemoveAsync((request as IClearCache).Key);
            return await next();
        }

        if ((request as IQueryCache<TResponse>).BypassCache)
        {
            return await next();
        }

        var key = (request as IQueryCache<TResponse>).CacheKey;
        return await cacheService.TryGetAndSet<TResponse>(key, next.Invoke,cancellationToken); 
    }
}