using Core.CQRS.Command;
using ProductSyncService.Domain.Categories;

namespace ProductSyncService.Application.Categories.Commands;


internal sealed class CreateCategoryCommandHandler(ICategoryRepository categoryRepository) : ICommandHandler<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return default;
    }
}