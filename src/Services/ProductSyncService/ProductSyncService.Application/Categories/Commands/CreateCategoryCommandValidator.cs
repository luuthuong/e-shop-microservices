using FluentValidation;

namespace ProductSyncService.Application.Categories.Commands;

internal sealed class CreateCategoryCommandValidator: AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        
    }
}