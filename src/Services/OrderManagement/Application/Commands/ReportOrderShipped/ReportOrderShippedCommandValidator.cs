using FluentValidation;

namespace OrderManagement.Application.Commands;

public class ReportOrderShippedCommandValidator : AbstractValidator<ReportOrderShippedCommand>
{
    public ReportOrderShippedCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderManagement ID is required");

        RuleFor(x => x.ShipmentId)
            .NotEmpty().WithMessage("Shipment ID is required");

        RuleFor(x => x.TrackingNumber)
            .NotEmpty().WithMessage("Tracking number is required");

        RuleFor(x => x.ShippedDate)
            .NotEmpty().WithMessage("Shipped date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Shipped date cannot be in the future");
    }
}