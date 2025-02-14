using Core.Infrastructure.EF.DbContext;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class OrderDbContext(DbContextOptions options) : BaseDbContext(options)
{
    public DbSet<Order> Orders { get; set; }
}