using Core.CQRS.Command;
using Domain.Payments;

namespace Application.Payments.CancellationPayment;

public record CancellationPaymentCommand(
    PaymentId PaymentId,
    PaymentCancelledReason Reason
) : ICommand;
