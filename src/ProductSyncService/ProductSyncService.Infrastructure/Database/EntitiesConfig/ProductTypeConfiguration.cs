using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductSyncService.Domain.Entities;

namespace ProductSyncService.Infrastructure.Database.EntitiesConfig;

public class ProductTypeConfiguration: IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.Property(x => x.Name).IsRequired();
        builder
            .HasMany(x => x.Products)
            .WithOne(x => x.ProductType)
            .HasForeignKey(p => p.ProductTypeId)
            .HasPrincipalKey(x => x.Id);
    }
}