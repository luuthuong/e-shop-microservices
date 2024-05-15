using Core.Domain;

namespace Core.Test;

internal sealed class DummyAggregateId(Guid value): StronglyTypeId<Guid>(value);

internal sealed class DummyAnotherAggregateId(Guid value): StronglyTypeId<Guid>(value);
