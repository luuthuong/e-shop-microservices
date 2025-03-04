using Core.Infrastructure.EF.DbContext;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Domain.Categories;
using ProductSyncService.Domain.Products;
using ProductSyncService.Domain.Quotes;

namespace ProductSyncService.Infrastructure.Persistence;

public sealed class ProductSyncDbContext(DbContextOptions options) : BaseDbContext(options)
{
    public DbSet<Product>? Product { get; set; }
    public DbSet<Category>? Category { get; set; }
    
    public DbSet<Quote>? Quotes { get; set; }
}