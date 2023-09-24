using Domain.DomainEvents.Users;
using MediatR;

namespace Application.CQRS.Users.Events;

internal sealed class UserCreatedDomainEventHandler: INotificationHandler<UserCreatedDomainEvent>
{
    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Send mail for user created");
        return Task.CompletedTask;
    }
}