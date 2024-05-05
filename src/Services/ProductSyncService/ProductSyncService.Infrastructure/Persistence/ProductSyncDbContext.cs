using Core.Infrastructure.EF.DbContext;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Products;

namespace ProductSyncService.Infrastructure.Persistence;

public sealed class ProductSyncDbContext: BaseDbContext
{
    public ProductSyncDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Product>? Product { get; set; }
    public DbSet<Category>? Category { get; set; }
}