using FluentValidation;

namespace OrderManagement.Application.Commands;

public class ReportStockReservedCommandValidator : AbstractValidator<ReportStockReservedCommand>
{
    public ReportStockReservedCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderManagement ID is required");
    }
}