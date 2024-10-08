using Core.Infrastructure.EF.Repository;
using ProductSyncService.Domain.Categories;

namespace ProductSyncService.Infrastructure.Persistence.Categories;

public class CategoryRepository: Repository<ProductSyncDbContext, Category, CategoryId>, ICategoryRepository
{
    public CategoryRepository(ProductSyncDbContext dbContext) : base(dbContext)
    {
    }
}