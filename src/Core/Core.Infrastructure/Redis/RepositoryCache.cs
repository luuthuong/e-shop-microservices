// using Core.Domain;
// using Core.Infrastructure.EF.DbContext;
// using Core.Infrastructure.EF.Repository;
// using Microsoft.Extensions.Caching.Distributed;
// using Newtonsoft.Json;
// using JsonConverter = Core.Infrastructure.Reflections.JsonConverter;
//
// namespace Core.Infrastructure.Redis;
//
// public abstract class RepositoryCache<TDbContext, TEntity>: Repository<TDbContext, TEntity> 
//     where TDbContext: BaseDbContext
// {
//     private readonly IDistributedCache _cache;
//     protected RepositoryCache(TDbContext dbContext, IDistributedCache cache) : base(dbContext)
//     {
//         _cache = cache;
//     }
//
//     protected async Task<T?> TryGetAndSet<T>(string key, Func<Task<T>> callback, CancellationToken cancellationToken = default)
//     {
//         var value = await _cache.GetStringAsync(key, cancellationToken);
//         if (string.IsNullOrEmpty(value))
//         {
//             var result = await callback();
//             if(result is  not null)
//                 await _cache.SetStringAsync(key, JsonConvert.SerializeObject(result), cancellationToken);
//             return result;
//         }
//
//         return JsonConverter.Parse<T>(value);
//     }
// }