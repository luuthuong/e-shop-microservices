using FluentValidation;

namespace Application.Payments.CreatePayment;

internal sealed class CreatePaymentValidator: AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentValidator()
    {
        
    }
}