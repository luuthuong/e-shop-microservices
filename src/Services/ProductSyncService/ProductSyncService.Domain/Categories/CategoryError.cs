using Core.Results;

namespace ProductSyncService.Domain.Categories;

public class CategoryError
{
    public static Error NameExisted => new("Category.NameExisted", "Category with name existed.");
}