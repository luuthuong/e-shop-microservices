using MediatR;

namespace Core.EventBus;

public interface IEventPublisher
{
    Task PublishAsync(INotification @event, CancellationToken cancellationToken = default);
}