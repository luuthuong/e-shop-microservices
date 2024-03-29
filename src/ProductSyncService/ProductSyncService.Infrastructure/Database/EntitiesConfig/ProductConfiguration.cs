using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductSyncService.Domain.Entities;

namespace ProductSyncService.Infrastructure.Database.EntitiesConfig;

public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name).IsRequired();
        // builder.HasOne(x => x.Type).WithMany().HasForeignKey(x => x.ProductTypeId);
    }
}