namespace Core.Test;

[TestFixture]
internal sealed class AggregateRootTest : TestBase
{
    [Test(Description = "TwoIdenticalAggregates_ShouldReturnEqualsFalse")]
    public void TwoIdenticalAggregates_ShouldReturnEqualsFalse()
    {

        Test(
            arrange: () =>
            {
                DummyAggregateId aggregateId = new(Guid.NewGuid());
                DummyAgreegateRoot aggregateRoot = new(aggregateId);
                DummyAgreegateRoot aggregateRootOther = new(aggregateId);
                return (aggregateRoot, aggregateRootOther);
            },
            act: (item) => item.aggregateRoot.GetHashCode() == item.aggregateRootOther.GetHashCode(),
            assert: Assert.IsFalse
        );
    }

    [Test]
    public void AppendDomains_ShouldReturnEqualUncommitedEventNumber()
    {
        int domainEventnumber = 5;

        Test(
            arrange: () =>
            {
                DummyAggregateId aggregateId = new(Guid.NewGuid());
                DummyAgreegateRoot agreegateRoot = new(aggregateId);
                return agreegateRoot;


            },
            act: (item) =>
            {
                for (int i = 0; i < domainEventnumber; i++)
                    item.DoSomething();
                return item;
            },
            assert: (item) => Assert.That(item.GetDomainEvents().Count, Is.EqualTo(domainEventnumber))
        );
    }
}
