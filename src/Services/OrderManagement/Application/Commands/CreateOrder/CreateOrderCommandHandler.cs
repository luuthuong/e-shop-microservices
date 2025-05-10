using Core.Domain;
using MediatR;
using OrderManagement.Domain;
using OrderManagement.Domain.Events;

namespace OrderManagement.Application.Commands;

public class CreateOrderCommandHandler(
    IEventStore<Domain.Order> orderEventStore,
    ILogger<CreateOrderCommandHandler> logger)
    : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new order for customer {CustomerId}", request.CustomerId);

        var orderItems = request.Items.Select(i => new OrderItem(
            i.ProductId,
            i.ProductName,
            i.Quantity,
            new Money(i.Price, request.Currency)
        )).ToList();

        Address shippingAddress = new()
        {
            Street = request.ShippingAddress.Street,
            City = request.ShippingAddress.City,
            State = request.ShippingAddress.State,
            ZipCode = request.ShippingAddress.ZipCode,
            Country = request.ShippingAddress.Country,
            PhoneNumber = request.ShippingAddress.PhoneNumber,
            RecipientName = request.ShippingAddress.RecipientName
        };

        var order = Domain.Order.Create(request.CustomerId, orderItems, shippingAddress);

        var orderCreatedEvent = order.GetUncommittedEvents().OfType<OrderCreatedEvent>().Single();

        await orderEventStore.AppendToOutboxAsync(orderCreatedEvent);
        
        await orderEventStore.SaveAsync(order);

        return new CreateOrderResponse(
            order.Id, 
            order.CustomerId,
            order.TotalAmount.Amount, 
            order.TotalAmount.Currency,
            order.Status.ToString(), 
            order.OrderDate
        );
    }
}