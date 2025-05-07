using FluentValidation;

namespace Ordering.Application.Commands;

public class ReportStockReservedCommandValidator : AbstractValidator<ReportStockReservedCommand>
{
    public ReportStockReservedCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Ordering ID is required");
    }
}