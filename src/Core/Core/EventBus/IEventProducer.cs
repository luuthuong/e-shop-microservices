using MediatR;

namespace Core.EventBus;

public interface IEventProducer
{
    Task DoAsync(INotification @event, CancellationToken cancellationToken = default);
}