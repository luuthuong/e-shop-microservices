using System.Text;
using Core.CQRS.Query;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using CacheSettings = Core.CQRS.Query.CacheSettings;
using Core.CQRS.Command;

namespace Core.Infrastructure.CQRS;

// comnad : delete cache

// query: try get and set

public class CachedBehavior<TRequest, TResponse>(
        IDistributedCache cache, 
        ILogger<TResponse> logger, 
        IOptions<CacheSettings> settings,
        ICacheService cacheService
) : IPipelineBehavior<TRequest, TResponse> where TRequest: IRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        if(request is IClearCache){
            await cacheService.RemoveAsync((request as IClearCache).Key);
            return await next();
        }

        if ((request as IQueryCache<TResponse>).BypassCache)
        {
            return await next();
        }

        return await next();
    }

    private async Task<TResponse> TryGetAndSet(string key, Func<Task<TResponse>> action, CancellationToken cancellationToken)
    {
        var cachedResponse = await cache.GetAsync(key, cancellationToken);

        if(cachedResponse is not null)
            return JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));

        var response = await action();
        var options = new DistributedCacheEntryOptions{};
        var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
        await cache.SetAsync(key, serializedData, options, cancellationToken);
        return response;
    }
}