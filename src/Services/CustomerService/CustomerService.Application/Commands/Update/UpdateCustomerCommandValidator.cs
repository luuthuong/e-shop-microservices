using FluentValidation;

namespace Application.Customers;

public class UpdateCustomerCommandValidator: AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(c => c.Name).NotNull().NotEmpty();
        
        RuleFor(c => c.ShippingAddress).NotNull().NotEmpty();

        RuleFor(c => c.CreditLimit).GreaterThan(0);
    }
}