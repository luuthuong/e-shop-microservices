using MediatR;

namespace PaymentProcessing.Application.Commands.RefundPayment;

public class RefundPaymentCommandHandler: IRequestHandler<RefundPaymentCommand, RefundPaymentResult>
{
    public Task<RefundPaymentResult> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}