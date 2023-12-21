using Core.Infrastructure.Redis;
using Microsoft.Extensions.Caching.Distributed;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Infrastructure.Persistence.Products;

public class ProductRepositoryCache : RepositoryCache<ProductSyncDbContext, Product>, IProductRepository
{
    private readonly IProductRepository _productRepository;

    public ProductRepositoryCache(ProductSyncDbContext dbContext, IDistributedCache cache,
        IProductRepository productRepository) : base(dbContext, cache)
    {
        _productRepository = productRepository;
    }

    public Task<IEnumerable<Product>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return _productRepository.GetListAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(ProductId productId, CancellationToken cancellationToken = default)
    {
        string key = $"product-{productId.Value}";
        return TryGetAndSet(
            key,
            () => _productRepository.GetByIdAsync(productId, cancellationToken)
            , cancellationToken
        );
    }
}