using Core.Exception;
using Core.Domain;

namespace Core.Test;

public class StronglyTypeIdTest
{
    [Test]
    public void EmptyGuid_ShouldThrowDomainException()
    {
        //Arrange
        var id = Guid.Empty;
        //Act
        var result = Assert.Throws<DomainRuleException>(
            () => new DummyAggregateId(id)
        );
        //Assert
        Assert.IsInstanceOf<DomainRuleException>(result);
    }

    [Test]
    public void TwoIdsWithDifferentTypes_ShouldReturnEqualFalse()
    {
        //Arrange
        var id = Guid.NewGuid();
        StronglyTypeId<Guid> strongId1 = new DummyAggregateId(id);
        StronglyTypeId<Guid> strongId2 = new DummyAnotherAggregateId(id);
        //Act
        var result = strongId1 == strongId2;
        //Assert
        Assert.That(strongId1, Is.Not.EqualTo(strongId2));
        Assert.IsFalse(result);
    }

    [Test]
    public void TwoIds_ShouldReturnEqualTrue()
    {
        //Arrange
        var id = Guid.NewGuid();
        StronglyTypeId<Guid> strongId1 = new DummyAggregateId(id);
        StronglyTypeId<Guid> strongId2 = new DummyAggregateId(id);
        //Act
        var result = strongId1 == strongId2;
        //Assert
        Assert.That(strongId1, Is.EqualTo(strongId2));
        Assert.IsTrue(result);
    }
}

