using Core.Domain;

namespace Core.Test;

internal sealed class DummyAgreegateRoot: AggregateRoot<DummyAggregateId>
{
    internal DummyAgreegateRoot(DummyAggregateId id) {
        Id = id;
    }

    public void DoSomething(){
        RaiseDomainEvent(new DummyDomainEvent());
    }
}
