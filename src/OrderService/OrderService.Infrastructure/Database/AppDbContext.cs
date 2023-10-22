using Core.BaseDbContext;
using Domain.Entities;
using Infrastructure.Database.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public sealed class AppDbContext: BaseDbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Category { get; set; }
}