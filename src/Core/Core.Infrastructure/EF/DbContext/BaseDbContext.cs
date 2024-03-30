using Core.EF;
using Core.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.EF.DbContext;

public abstract class BaseDbContext(DbContextOptions options)
    : Microsoft.EntityFrameworkCore.DbContext(options), IDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public async ValueTask<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<OutboxMessage> OutboxMessage { get; set; }
    public DbSet<SeedingHistory> SeedingHistory { get; set; }
}