using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Infrastructure.Persistence.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                p => p.Value,
                p => ProductId.From(p));

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .IsRequired();

        builder.OwnsOne(
            e => e.Price,
            b =>
            {
                b.Property(e => e.Amount)
                    .HasColumnName("Price")
                    .HasColumnType("decimal(18,2)");

                b.OwnsOne(
                    e => e.Currency,
                    bc =>
                    {
                        bc.Property(e => e.Code)
                            .HasColumnName("CurrencyCode")
                            .HasMaxLength(5)
                            .IsRequired();

                        bc.Property(e => e.Symbol)
                            .HasColumnName("CurrencySymbol")
                            .HasMaxLength(5);
                    });
            });
    }
}