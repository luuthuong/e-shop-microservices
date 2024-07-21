using Core.Infrastructure.EF.Repository;

namespace Core.Test.Dummy.EF;

public class DummyRepository(DummyDbContext dbContext) : Repository<DummyDbContext, DummyAgreegateRoot, DummyAggregateId>(dbContext)
{
}