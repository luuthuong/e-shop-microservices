using Core.Infrastructure.EF.DbContext;

namespace Core.Infrastructure.EF.Repository;

public class OutboxEventRepository<TDbContext>(TDbContext dbContext) 
    where TDbContext : BaseDbContext
{
    
}