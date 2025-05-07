using System.Transactions;
using Core.Domain;
using Core.EF;

namespace Core.Infrastructure.EF.Projections;

public delegate void ProjectEvent<in TEvent,in T>(TEvent @event, T item) where T : class where TEvent: DomainEvent;

public abstract class AbstractStreamProjection<T> : IStreamProjection where T : class, new()
{
    private readonly Dictionary<Type, Func<T, DomainEvent, T>> _handlers = new();

    public IEnumerable<Type> HandledEventTypes => _handlers.Keys;

    protected void ProjectEvent<TEvent>(ProjectEvent<TEvent, T> handler) where TEvent : DomainEvent
    {
        _handlers[typeof(TEvent)] = (item, @event) =>
        {
            if (@event is TEvent typedEvent)
            {
                handler(typedEvent, item);
            }

            return item;
        };
    }

    public virtual async Task ProjectEvent(DomainEvent @event, CancellationToken cancellationToken = default)
    {
        if (!_handlers.TryGetValue(@event.GetType(), out var handler))
            return;

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var item = await LoadOrCreateItemAsync(@event.AggregateId, cancellationToken);
        item = handler(item, @event);
        await SaveItemAsync(item, cancellationToken);
        scope.Complete();
    }
    
    protected bool Exists<TEvent>(TEvent @event) where TEvent : DomainEvent
    {
        return _handlers.ContainsKey(@event.GetType());
    }

    protected Func<T, DomainEvent, T> GetHandler<TEvent>(TEvent @event) where TEvent : DomainEvent
    {
        if (_handlers.TryGetValue(@event.GetType(), out var handler))
            return handler;

        throw new InvalidOperationException($"No handler found for event type {@event.GetType()}");
    }

    protected abstract Task<T> LoadOrCreateItemAsync(Guid id, CancellationToken cancellationToken = default);
    protected abstract Task SaveItemAsync(T item, CancellationToken cancellationToken = default);
}