using Core.EventBus;
using Domain.Events;

namespace Application.Events;

public class CustomerCreatedDomainEvent: IEventHandler<CustomerCreated>
{
    public Task Handle(CustomerCreated notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}