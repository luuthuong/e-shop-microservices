using Microsoft.EntityFrameworkCore;
using OrderManagement.Infrastructure.Models;

namespace OrderManagement.Infrastructure;

public class OrderReadDbContext(DbContextOptions<OrderReadDbContext> options) : DbContext(options)
{
    public DbSet<OrderReadModel> Orders { get; set; }
    public DbSet<PaymentReadModel> Payments { get; set; }
    public DbSet<ShipmentReadModel> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // OrderManagement
        modelBuilder.Entity<OrderReadModel>(builder =>
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Status).HasMaxLength(50).IsRequired();
            builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(o => o.Currency).HasMaxLength(3).IsRequired();
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.LastUpdated).IsRequired();
            builder.Property(o => o.Version).IsRequired();

            // Relationships
            builder.OwnsMany(o => o.Items, itemBuilder =>
            {
                itemBuilder.ToTable("OrderItems");
                itemBuilder.WithOwner().HasForeignKey(i => i.OrderId);
                itemBuilder.Property(oi => oi.ProductName).HasMaxLength(255).IsRequired();
                itemBuilder.Property(oi => oi.Quantity).IsRequired();
                itemBuilder.Property(oi => oi.Price).HasColumnType("decimal(18,2)").IsRequired();
                itemBuilder.Property(oi => oi.Currency).HasMaxLength(3).IsRequired();
                itemBuilder.HasIndex(oi => oi.OrderId);
            });

            builder.OwnsOne(o => o.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(sa => sa.Street).HasMaxLength(255).IsRequired().HasColumnName("Street");
                addressBuilder.Property(sa => sa.City).HasMaxLength(100).IsRequired().HasColumnName("City");
                addressBuilder.Property(sa => sa.State).HasMaxLength(100).IsRequired().HasColumnName("State");
                addressBuilder.Property(sa => sa.Country).HasMaxLength(100).IsRequired().HasColumnName("Country");
                addressBuilder.Property(sa => sa.ZipCode).HasMaxLength(20).IsRequired().HasColumnName("ZipCode");
                addressBuilder.Property(sa => sa.RecipientName).HasMaxLength(255).IsRequired().HasColumnName("RecipientName");
                addressBuilder.Property(sa => sa.PhoneNumber).HasMaxLength(50).IsRequired().HasColumnName("PhoneNumber");
            });

            builder.OwnsMany(o => o.Metadata, metadataBuilder =>
            {
                metadataBuilder.ToTable("OrderMetadata");
                metadataBuilder.WithOwner().HasForeignKey(om => om.OrderId);
                metadataBuilder.Property(om => om.Key).HasMaxLength(255).IsRequired();
                metadataBuilder.Property(om => om.Value).HasMaxLength(4000);
                metadataBuilder.HasIndex(om => new { om.OrderId, om.Key }).IsUnique();
            });
            
            builder.OwnsMany(o => o.History, historyBuilder =>
            {
                historyBuilder.ToTable("OrderHistory");
                historyBuilder.WithOwner().HasForeignKey(oh => oh.OrderId);
                historyBuilder.Property(oh => oh.Status).HasMaxLength(50).IsRequired();
                historyBuilder.Property(oh => oh.Description).HasMaxLength(1000);
                historyBuilder.Property(oh => oh.Timestamp).IsRequired();
                historyBuilder.HasIndex(oh => oh.OrderId );
            });
        });
        
        // PaymentProcessing
        modelBuilder.Entity<PaymentReadModel>(builder =>
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Status).HasMaxLength(50).IsRequired();
            builder.Property(p => p.TransactionId).HasMaxLength(255);
            builder.Property(p => p.Method).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Currency).HasMaxLength(3).IsRequired();

            // Create unique index for OrderId
            builder.HasIndex(p => p.OrderId).IsUnique();
        });

        // Shipment
        modelBuilder.Entity<ShipmentReadModel>(builder =>
        {
            builder.ToTable("Shipments");
            builder.HasKey(p => p.Id);
            builder.Property(s => s.Status).HasMaxLength(50).IsRequired();
            builder.Property(s => s.TrackingNumber).HasMaxLength(100);
            builder.Property(s => s.Carrier).HasMaxLength(50).IsRequired();

            // Create unique index for OrderId
            builder.HasIndex(s => s.OrderId).IsUnique();
        });
    }
}