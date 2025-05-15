using MediatR;
using PaymentProcessing.Application.Commands.ProcessPayment;
using PaymentProcessing.Domain;

namespace PaymentProcessing.Application.Commands.ProcessingPayment;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, ProcessPaymentResult>
{
    public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        // Create a new payment aggregate
        var payment = PaymentAggregate.Create(
            request.OrderId,
            request.CustomerId,
            request.Amount,
            request.Currency,
            request.PaymentMethod);

        return default;
    }
}