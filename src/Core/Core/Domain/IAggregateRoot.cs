namespace Core.Domain;

public interface IAggregateRoot
{
    void RaiseDomainEvent(IDomainEvent @event);
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}