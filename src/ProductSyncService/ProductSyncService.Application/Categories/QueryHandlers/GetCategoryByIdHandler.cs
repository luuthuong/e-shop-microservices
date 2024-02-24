using Core.CQRS.Query;
using ProductSyncService.Application.Categories.Queries;
using ProductSyncService.Domain.Categories;
using ProductSyncService.DTO.Categories;

namespace ProductSyncService.Application.Categories.QueryHandlers;

public class GetCategoryByIdHandler(ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoryById, CategoryDTO>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public Task<CategoryDTO> Handle(GetCategoryById request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}