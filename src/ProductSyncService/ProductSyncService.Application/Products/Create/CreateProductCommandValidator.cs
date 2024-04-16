using FluentValidation;

namespace ProductSyncService.Application.Products;

public sealed class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Description).NotEmpty();
    }
}