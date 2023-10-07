using Core.BaseDbContext;
using Microsoft.EntityFrameworkCore;
using ProductSyncService.Domain.Entities;

namespace ProductSyncService.Infrastructure.Database.Interfaces;

public interface IAppDbContext: IBaseDbContext
{
    DbSet<Product> Product { get; set; }
    DbSet<ProductType> ProductType { get; set; }
}