using Core.Domain;

namespace Core.Test;

public sealed class DummyAgreegateRoot: AggregateRoot<DummyAggregateId>
{
    internal DummyAgreegateRoot(DummyAggregateId id) {
        Id = id;
    }

    private DummyAgreegateRoot(){

    }

    public static DummyAgreegateRoot Create(){
        return new DummyAgreegateRoot(){
            CreatedDate = DateTime.Now,
            Id = new DummyAggregateId(Guid.NewGuid()),
        };
    }

    public void DoSomething(){
        RaiseDomainEvent(new DummyDomainEvent());
    }
}
