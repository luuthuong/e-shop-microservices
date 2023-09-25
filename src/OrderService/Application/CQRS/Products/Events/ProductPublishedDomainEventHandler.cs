using Domain.DomainEvents.Products;
using MediatR;

namespace Application.CQRS.Products.Events;

public sealed class ProductPublishedDomainEventHandler: INotificationHandler<PublishProductDomainEvent>
{
    public Task Handle(PublishProductDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Publish product domain event handler.");
        return Task.CompletedTask;
    }
}