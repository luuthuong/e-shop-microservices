namespace Core.BaseDomain;

public interface IAggregateRoot
{
    void RaiseDomainEvent(IDomainEvent @event);
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}