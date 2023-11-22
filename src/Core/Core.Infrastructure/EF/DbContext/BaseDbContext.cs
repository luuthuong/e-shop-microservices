using Core.EF;
using Core.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Core.Infrastructure.EF.DbContext;

public abstract class BaseDbContext: Microsoft.EntityFrameworkCore.DbContext, IDbContext
{
    protected BaseDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
    public async ValueTask<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<OutboxMessage> OutboxMessage { get; set; }
    public DbSet<SeedingHistory> SeedingHistory { get; set; }
    public override DatabaseFacade Database => base.Database;

}