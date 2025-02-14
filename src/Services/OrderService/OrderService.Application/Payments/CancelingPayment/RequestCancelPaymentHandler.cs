using Core.CQRS.Command;
using EcommerceDDD.OrderProcessing.Application.Payments.CancelingPayment;

namespace Application.Payments.CancelingPayment;

public class RequestCancelPaymentHandler: ICommandHandler<RequestCancelPayment>
{
    public async Task Handle(RequestCancelPayment command, CancellationToken cancellationToken)
    {}
}

public record class CancelPaymentRequest(int PaymentCancellationReason);