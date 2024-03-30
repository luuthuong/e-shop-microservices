using FluentValidation;

namespace ProductSyncService.Application.Products.Commands;

public sealed class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Description).NotEmpty();
    }
}