using Core.BaseDbContext;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Domain.Entities;
using ProductSyncService.Infrastructure.Database.Interfaces;

namespace ProductSyncService.Infrastructure.Database;

public class AppDbContext: BaseDbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Product { get; set; }
    public DbSet<ProductType> ProductType { get; set; }
}