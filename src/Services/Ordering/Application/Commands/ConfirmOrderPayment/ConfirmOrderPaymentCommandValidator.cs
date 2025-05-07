using FluentValidation;

namespace Ordering.Application.Commands;

public class ConfirmOrderPaymentCommandValidator : AbstractValidator<ConfirmOrderPaymentCommand>
{
    public ConfirmOrderPaymentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Ordering ID is required");

        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("PaymentProcessing ID is required");

        RuleFor(x => x.TransactionId)
            .NotEmpty().WithMessage("Transaction ID is required")
            .MaximumLength(100).WithMessage("Transaction ID cannot exceed 100 characters");
    }
}