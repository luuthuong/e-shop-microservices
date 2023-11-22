using Core.Infrastructure.EF.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public sealed class AppDbContext: BaseDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}