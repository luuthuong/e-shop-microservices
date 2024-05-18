using Core.Domain;

namespace Core.Test;

public sealed class DummyAggregateId(Guid value): StronglyTypeId<Guid>(value);

public sealed class DummyAnotherAggregateId(Guid value): StronglyTypeId<Guid>(value);
