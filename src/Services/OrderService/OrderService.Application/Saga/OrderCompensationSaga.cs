using Application.Events;
using Application.Events.Inventories;
using Application.Events.Payments;
using Core.EventBus;

namespace Application.Saga;

public class OrderCompensationSaga: 
    IEventHandler<PaymentFailed>,
    IEventHandler<CustomerCreditLimit>,
    IEventHandler<ProductOutOfStock>,
    IEventHandler<OrderCancelled>

{
    public Task Handle(PaymentFailed notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Handle(CustomerCreditLimit notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Handle(ProductOutOfStock notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Handle(OrderCancelled notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}