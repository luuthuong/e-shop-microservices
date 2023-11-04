using ProductSyncService.Domain.Entities;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Infrastructure.Database.Repository;

public sealed class ProductTypeRepository: AppRepository<ProductType>, IProductTypeRepository
{
    public ProductTypeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}