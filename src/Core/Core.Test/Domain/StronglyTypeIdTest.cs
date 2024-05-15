using Core.Exception;
using Core.Domain;

namespace Core.Test;

internal class StronglyTypeIdTest : TestBase
{
    [Test]
    public void EmptyGuid_ShouldThrowDomainException()
    {

        Test(
            arrange: () => Guid.Empty,
            act: (item) => Assert.Throws<DomainRuleException>(
                () => new DummyAggregateId(item)
            ),
            assert: Assert.IsInstanceOf<DomainRuleException>
        );
    }

    [Test]
    public void TwoIdsWithDifferentTypes_ShouldReturnEqualFalse()
    {
        Test(
            arrange: () =>
            {
                var id = Guid.NewGuid();
                StronglyTypeId<Guid> strongId1 = new DummyAggregateId(id);
                StronglyTypeId<Guid> strongId2 = new DummyAnotherAggregateId(id);
                return (id1: strongId1, id2: strongId2);
            },
            act: (item) => item.id1 == item.id2,
            assert: (arrange, actResult) =>
            {
                Assert.IsFalse(actResult);
                Assert.That(arrange.id1, Is.Not.EqualTo(arrange.id2));
            }
        );
    }

    [Test]
    public void TwoIds_ShouldReturnEqualTrue()
    {
        Test(
            arrange: () =>
            {
                var id = Guid.NewGuid();
                StronglyTypeId<Guid> strongId1 = new DummyAggregateId(id);
                StronglyTypeId<Guid> strongId2 = new DummyAggregateId(id);
                return (id1: strongId1, id2: strongId2);
            },
            act: (item) => item.id1 == item.id2,
            assert: (arrange, actResult) =>
            {
                Assert.IsTrue(actResult);
                Assert.That(arrange.id1, Is.EqualTo(arrange.id2));
            }
        );
    }
}

