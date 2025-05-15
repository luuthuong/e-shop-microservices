using Core.Domain;
using MediatR;

namespace Core.EventBus;

public interface IEventHandler<in T> where T : DomainEvent
{
    Task HandleAsync(T @event);
}