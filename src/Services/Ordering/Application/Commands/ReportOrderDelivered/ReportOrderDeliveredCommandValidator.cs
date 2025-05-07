using FluentValidation;

namespace Ordering.Application.Commands;

public class ReportOrderDeliveredCommandValidator : AbstractValidator<ReportOrderDeliveredCommand>
{
    public ReportOrderDeliveredCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Ordering ID is required");

        RuleFor(x => x.DeliveredDate)
            .NotEmpty().WithMessage("Delivered date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Delivered date cannot be in the future");
    }
}