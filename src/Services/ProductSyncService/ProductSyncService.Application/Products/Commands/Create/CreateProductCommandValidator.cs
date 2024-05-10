using FluentValidation;

namespace ProductSyncService.Application.Products;

public sealed class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().MinimumLength(10);
        RuleFor(x => x.Description).NotEmpty();
    }
}