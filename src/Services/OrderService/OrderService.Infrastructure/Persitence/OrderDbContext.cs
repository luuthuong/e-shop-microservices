using Core.Infrastructure.EF.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persitence;

public sealed class OrderDbContext: BaseDbContext
{
    public OrderDbContext(DbContextOptions options) : base(options)
    {
    }
}