using Core.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Core.EF;

public interface IDbContext
{
    public DbSet<OutboxMessage> OutboxMessage { get; set; }
    DatabaseFacade Database { get; }
    ValueTask<int> SaveChangeAsync(CancellationToken cancellationToken = default);
}