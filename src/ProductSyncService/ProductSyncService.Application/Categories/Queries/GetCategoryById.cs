using Core.CQRS.Query;
using ProductSyncService.Domain.Categories;
using ProductSyncService.DTO.Categories;

namespace ProductSyncService.Application.Categories.Queries;

public class GetCategoryById: IQuery<CategoryDTO>
{
    private GetCategoryById(CategoryId categoryId, CancellationToken cancellationToken)
    {
        CategoryId = categoryId;
        CancellationToken = cancellationToken;
    }

    public CategoryId CategoryId { get; private set; }
    public  CancellationToken CancellationToken { get; private set; }

    public static GetCategoryById Create(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default) 
        => new(categoryId, cancellationToken);
}