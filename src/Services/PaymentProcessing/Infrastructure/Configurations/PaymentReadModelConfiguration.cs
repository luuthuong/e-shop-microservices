using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentProcessing.Infrastructure.Models;

namespace PaymentProcessing.Infrastructure.Configurations;

public class PaymentReadModelConfiguration : IEntityTypeConfiguration<PaymentReadModel>
{
    public void Configure(EntityTypeBuilder<PaymentReadModel> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Status).HasMaxLength(50).IsRequired();
        builder.Property(p => p.TransactionId).HasMaxLength(255);
        builder.Property(p => p.Method).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.Currency).HasMaxLength(3).IsRequired();
        builder.HasIndex(p => p.OrderId).IsUnique();
    }
}