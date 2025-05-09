using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure.Models;

namespace ProductCatalog.Infrastructure;

public class ProductCatalogReadDbContext(DbContextOptions<ProductCatalogReadDbContext> options) : DbContext(options)
{
    public DbSet<ProductReadModel> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var productBuilder = modelBuilder.Entity<ProductReadModel>();
        
        productBuilder.ToTable("Products");
        
        productBuilder.HasIndex(p => p.Name);
        productBuilder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        productBuilder.HasIndex(p => p.Category);
    }
}