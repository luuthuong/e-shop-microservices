using FluentValidation;

namespace OrderManagement.Application.Commands;

public class ReportOrderDeliveredCommandValidator : AbstractValidator<ReportOrderDeliveredCommand>
{
    public ReportOrderDeliveredCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderManagement ID is required");

        RuleFor(x => x.DeliveredDate)
            .NotEmpty().WithMessage("Delivered date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Delivered date cannot be in the future");
    }
}