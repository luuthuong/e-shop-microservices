using FluentValidation;

namespace ProductSyncService.Application.Categories;

internal sealed class CreateCategoryCommandValidator: AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        
    }
}