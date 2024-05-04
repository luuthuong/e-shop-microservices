using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductSyncService.Domain.Categories;

namespace ProductSyncService.Infrastructure.Persistence.Categories;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                x => x.Value,
                x => CategoryId.From(x)
            );

        builder.Property(c => c.Name).IsRequired();
        builder.HasIndex(c => c.Name).IsUnique();
        builder.HasMany(c => c.Products)
            .WithOne()
            .HasForeignKey(p => p.CategoryId);
    }
}