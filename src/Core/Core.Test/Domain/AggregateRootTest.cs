namespace Core.Test;

[TestFixture]
public sealed class AggregateRootTest
{
    [Test]
    public void TwoIdenticalAggregates_ShouldReturnEqualsFalse()
    {
        //Arrange
        DummyAggregateId aggregateId = new(Guid.NewGuid());
        DummyAgreegateRoot aggregateRoot = new(aggregateId);
        DummyAgreegateRoot aggregateRootOther = new(aggregateId);
        //Act
        var result = aggregateRoot.GetHashCode() == aggregateRootOther.GetHashCode();
        //Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void AppendDomains_ShouldReturnEqualUncommitedEventNumber()
    {
        //Arrange
        int domainEventnumber = 5;
        DummyAggregateId aggregateId = new(Guid.NewGuid());
        DummyAgreegateRoot agreegateRoot = new(aggregateId);
        //Act
        for (int i = 0; i < domainEventnumber; i++)
            agreegateRoot.DoSomething();
        //Assert
        Assert.That(agreegateRoot.GetDomainEvents().Count, Is.EqualTo(domainEventnumber));
    }
}
