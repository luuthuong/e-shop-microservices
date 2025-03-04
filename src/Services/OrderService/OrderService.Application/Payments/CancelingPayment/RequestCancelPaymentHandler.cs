using Core.CQRS.Command;

namespace Application.Payments.CancelingPayment;

public class RequestCancelPaymentHandler: ICommandHandler<RequestCancelPayment>
{
    public async Task Handle(RequestCancelPayment command, CancellationToken cancellationToken)
    {}
}

public record class CancelPaymentRequest(int PaymentCancellationReason);