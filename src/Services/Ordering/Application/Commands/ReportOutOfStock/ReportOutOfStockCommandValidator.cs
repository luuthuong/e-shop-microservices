using FluentValidation;

namespace Ordering.Application.Commands;

public class ReportOutOfStockCommandValidator : AbstractValidator<ReportOutOfStockCommand>
{
    public ReportOutOfStockCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Ordering ID is required");

        RuleFor(x => x.OutOfStockProductIds)
            .NotEmpty().WithMessage("At least one out-of-stock product ID must be specified");
    }
}