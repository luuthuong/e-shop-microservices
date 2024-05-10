using Core.EventBus;
using Domain.Events;

namespace Application.Events;

public class CustomerCreatedDomainEvent: IEventHandler<CustomerCreated>
{
    public Task Handle(CustomerCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Send mail to {notification.Email}");
        return Task.CompletedTask;
    }
}