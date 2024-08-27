using Microsoft.EntityFrameworkCore;
using Core.Infrastructure.EF.DbContext;

namespace Core.Test.Dummy.EF;

public class DummyDbContext : BaseDbContext
{
    public DbSet<DummyAgreegateRoot> DummyAgreegateRoots { get; set; }
    public DummyDbContext(DbContextOptions<DummyDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DummyAgreegateRoot>().Property(x => x.Id).HasConversion(
            v => v.Value,
            v => new DummyAggregateId(v)
        );
        base.OnModelCreating(modelBuilder);
    }
}