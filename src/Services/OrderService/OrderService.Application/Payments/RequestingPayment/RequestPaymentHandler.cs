using Core.CQRS.Command;

namespace Application.Payments.RequestingPayment;

public class RequestPaymentHandler : ICommandHandler<RequestPayment>
{
    public async Task Handle(RequestPayment command, CancellationToken cancellationToken)
    {}
}

public record class PaymentRequest(
    Guid CustomerId,
    Guid OrderId,
    decimal TotalAmount,
    string currencyCode);
