using Application.Events;
using Application.Events.Payments;
using Core.EventBus;

namespace Application.Saga;

public class OrderSaga: 
    IEventHandler<OrderPlaced>,
    IEventHandler<OrderProcessed>,
    IEventHandler<PaymentSucceed>
{
    public Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Handle(OrderProcessed notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Handle(PaymentSucceed notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}