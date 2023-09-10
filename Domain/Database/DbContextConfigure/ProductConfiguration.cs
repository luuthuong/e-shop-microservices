using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Database.DbContextConfigure;

public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsOne(x => x.Price, priceBuilder =>
        {
            priceBuilder.Property(p => p.Concurrency).HasMaxLength(5);
        });
    }
}