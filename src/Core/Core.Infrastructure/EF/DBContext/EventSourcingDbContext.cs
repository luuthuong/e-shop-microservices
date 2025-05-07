using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.EF.DBContext;

public class EventSourcingDbContext(DbContextOptions<EventSourcingDbContext> options)
    : DbContext(options)
{
    public DbSet<EventData> Events { get; set; }
    public DbSet<IntegrationEvent> OutboxEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventData>(builder =>
        {
            builder.ToTable("Events");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.AggregateId).IsRequired();
            builder.Property(e => e.Type).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Version).IsRequired();
            builder.Property(e => e.Data).IsRequired();
            builder.Property(e => e.Timestamp).IsRequired();

            // Indexes for efficient event retrieval
            builder.HasIndex(e => new { e.AggregateId, e.Version }).IsUnique();
            builder.HasIndex(e => e.Timestamp);
        });
    }
}