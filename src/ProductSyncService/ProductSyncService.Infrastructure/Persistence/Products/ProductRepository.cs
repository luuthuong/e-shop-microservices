using Core.Infrastructure.EF.Repository;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Infrastructure.Persistence.Products;

public sealed class ProductRepository: Repository<ProductSyncDbContext, Product>, IProductRepository
{
    public ProductRepository(ProductSyncDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Product>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await DBSet.ToListAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(ProductId productId, CancellationToken cancellationToken = default)
    {
        return DBSet.FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);
    }
}