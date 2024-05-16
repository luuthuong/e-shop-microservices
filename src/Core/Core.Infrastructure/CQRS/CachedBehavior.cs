using System.Text;
using Core.CQRS.Query;
using MassTransit.Caching;
using MediatR;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using CacheSettings = Core.CQRS.Query.CacheSettings;

namespace Core.Infrastructure.CQRS;

public class CachedBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest: IQueryCache<TResponse>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger _logger;
    private readonly CacheSettings _settings;

    public CachedBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> settings)
    {
        _cache = cache;
        _logger = logger;
        _settings = settings.Value;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response;
        if (request.BypassCache)
        {
            return await next();
        }

        async Task<TResponse> GetResponseAndAddToCache()
        {
            response = await next();
            var options = new DistributedCacheEntryOptions{};
            var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
            await _cache.SetAsync((string)request.CacheKey, serializedData, options, cancellationToken);
            return response;
        }

        var cachedResponse = await _cache.GetAsync((string)request.CacheKey, cancellationToken);
        if (cachedResponse != null)
        {
            response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
            _logger.LogInformation($"Fetch from Cached -> '{request.CacheKey}'");
        }
        else
        {
            response = await GetResponseAndAddToCache();
            _logger.LogInformation($"Add to Cached -> '{request.CacheKey}'");
        }
        return response;
    }
}