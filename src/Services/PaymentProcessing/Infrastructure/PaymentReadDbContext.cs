using Microsoft.EntityFrameworkCore;
using PaymentProcessing.Infrastructure.Models;

namespace PaymentProcessing.Infrastructure;

public class PaymentReadDbContext(DbContextOptions<PaymentReadDbContext> options) : DbContext(options)
{
    public DbSet<PaymentReadModel> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentReadModel>(entity =>
        {
            entity.ToTable("Payments");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OrderId).IsRequired();
            entity.Property(e => e.CustomerId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Currency).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Provider).IsRequired();
            entity.Property(e => e.TransactionId).HasMaxLength(100);
            entity.Property(e => e.FailureReason).HasMaxLength(500);
            entity.Property(e => e.ErrorCode).HasMaxLength(50);
            entity.Property(e => e.RefundId).HasMaxLength(100);
            entity.Property(e => e.RefundAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.RefundReason).HasMaxLength(500);

            // Indexes for faster queries
            entity.HasIndex(e => e.OrderId).IsUnique();
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ProcessedDate);
        });
    }
}