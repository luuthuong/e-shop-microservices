using Core.BaseDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Core.BaseDbContext;

public interface IBaseDbContext
{
    public DbSet<OutboxMessage> OutboxMessage { get; set; }
    DatabaseFacade Database { get; }
    ValueTask<int> SaveChangeAsync(CancellationToken cancellationToken = default);
}