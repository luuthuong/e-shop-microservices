using Core.Domain;

namespace Core.EF;

public interface IProjection
{
    Task ProjectEvent(DomainEvent @event, CancellationToken cancellationToken = default);
}

public interface IStreamProjection : IProjection 
{
    IEnumerable<Type> HandledEventTypes { get; }
}