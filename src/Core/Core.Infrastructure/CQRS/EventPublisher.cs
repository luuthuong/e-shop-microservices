using Core.EventBus;
using MediatR;

namespace Core.Infrastructure.CQRS;

public class EventPublisher(IMediator mediator): IEventPublisher
{
    public Task PublishAsync(INotification @event, CancellationToken cancellationToken = default)
    {
        return mediator.Publish(@event, cancellationToken);
    }
}