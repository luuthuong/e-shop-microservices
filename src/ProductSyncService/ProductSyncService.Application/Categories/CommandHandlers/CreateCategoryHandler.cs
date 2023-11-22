using Core.CQRS.Command;
using ProductSyncService.Application.Categories.Commands;
using ProductSyncService.Domain.Categories;

namespace ProductSyncService.Application.Categories.CommandHandlers;

public class CreateCategoryHandler: ICommandHandler<CreateCategory>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task Handle(CreateCategory request, CancellationToken cancellationToken)
    {
        return default;
    }
}