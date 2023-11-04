using Microsoft.EntityFrameworkCore;
using ProductSyncService.Domain.Entities;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Infrastructure.Database.Repository;

public sealed class ProductRepository: AppRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Product>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await DBSet.Include(p => p.ProductType).ToListAsync(cancellationToken);
    }
}