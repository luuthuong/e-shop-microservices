using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(
                o => o.Value,
                v => OrderId.From(v)
            );


        builder.Property(e => e.CustomerId)
            .HasConversion(
                o => o.Value,
                v => CustomerId.From(v)
            ).IsRequired();

        builder.Property(e => e.QuoteId)
            .HasConversion(
                o => o.Value,
                v => QuoteId.From(v)
            ).IsRequired();

        builder.Property(e => e.PaymentId)
            .HasConversion(
                o => o.Value,
                v => PaymentId.From(v)
            ).IsRequired();

        builder.Property(e => e.ShipmentId)
            .HasConversion(
                o => o.Value,
                v => ShipmentId.From(v)
            ).IsRequired();

        builder.OwnsMany(e => e.OrderLines,
            b =>
            {
                b.OwnsOne(
                    ol => ol.ProductItem,
                    ob =>
                    {
                        ob.Property(p => p.ProductId)
                            .HasConversion(
                                p => p.Value,
                                v => ProductId.From(v))
                            .IsRequired();

                        ob.OwnsOne(
                            p => p.UnitPrice,
                            pb =>
                            {
                                pb.Property(m => m.Amount)
                                    .HasColumnType("decimal(18,2)");

                                pb.OwnsOne(m => m.Currency,
                                    cb =>
                                    {
                                        cb.Property(e => e.Code)
                                            .HasColumnName("CurrencyCode")
                                            .HasMaxLength(5)
                                            .IsRequired();

                                        cb.Property(e => e.Symbol)
                                            .HasColumnName("CurrencySymbol")
                                            .HasMaxLength(5);
                                    });
                            });

                        ob.OwnsOne(
                            p => p.Currency,
                            cb =>
                            {
                                cb.Property(e => e.Code)
                                    .HasColumnName("CurrencyCode")
                                    .HasMaxLength(5)
                                    .IsRequired();

                                cb.Property(e => e.Symbol)
                                    .HasColumnName("CurrencySymbol")
                                    .HasMaxLength(5);
                            });
                    }
                );
            });

        builder.OwnsOne(
            o => o.TotalPrice,
            pb =>
            {
                pb.Property(m => m.Amount)
                    .HasColumnType("decimal(18,2)").HasColumnName("TotalPrice");

                pb.OwnsOne(m => m.Currency,
                    cb =>
                    {
                        cb.Property(e => e.Code)
                            .HasColumnName("CurrencyCode")
                            .HasMaxLength(5)
                            .IsRequired();

                        cb.Property(e => e.Symbol)
                            .HasColumnName("CurrencySymbol")
                            .HasMaxLength(5);
                    });
            }
        );

        builder
            .Property(o => o.Status)
            .HasConversion(
                o => o.ToString(),
                v => Enum.Parse<OrderStatus>(v)
            );
    }
}