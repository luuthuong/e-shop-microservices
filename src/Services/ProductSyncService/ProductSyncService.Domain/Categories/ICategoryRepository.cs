using Core.EF;

namespace ProductSyncService.Domain.Categories;

public interface ICategoryRepository: IRepository<Category, CategoryId>
{
}