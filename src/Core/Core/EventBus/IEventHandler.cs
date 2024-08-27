using Core.Domain;
using MediatR;

namespace Core.EventBus;

public interface IEventHandler<in TEvent>: INotificationHandler<TEvent> where TEvent: INotification
{
}