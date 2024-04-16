using FluentValidation;

namespace Application.Commands.Customers;

internal sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {

        RuleFor(c => c.Name).NotNull().NotEmpty();
        
        RuleFor(c => c.Email).NotEmpty().NotNull();

        RuleFor(c => c.Password).NotNull().NotEmpty().MinimumLength(5);

        RuleFor(c => c.PasswordConfirm).NotNull().NotEmpty();

        RuleFor(c => c.ShippingAddress).NotNull().NotEmpty();

        RuleFor(c => c.CreditLimit).GreaterThan(0);
    }
}
