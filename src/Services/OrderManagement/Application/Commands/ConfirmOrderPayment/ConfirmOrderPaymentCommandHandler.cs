
using Core.Domain;
using Core.Exception;
using MediatR;
using OrderManagement.Domain;

namespace OrderManagement.Application.Commands;

public class ConfirmOrderPaymentCommandHandler(
    IEventStore<Domain.Order> orderEventStore,
    ILogger<ConfirmOrderPaymentCommandHandler> logger)
    : IRequestHandler<ConfirmOrderPaymentCommand>
{

    public async Task Handle(ConfirmOrderPaymentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Confirming payment for order {OrderId}, payment {PaymentId}, transaction {TransactionId}", 
            request.OrderId, request.PaymentId, request.TransactionId);

        // Get the order
        var order = await orderEventStore.LoadAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException($"OrderManagement with ID {request.OrderId} not found");
        }

        // Add payment information to order metadata
        order.AddMetadata("PaymentId", request.PaymentId.ToString());
        order.AddMetadata("TransactionId", request.TransactionId);

        // Confirm payment
        order.ConfirmPayment();

        await orderEventStore.SaveAsync(order);

        logger.LogInformation("PaymentProcessing confirmed for order {OrderId}", request.OrderId);
    }
}