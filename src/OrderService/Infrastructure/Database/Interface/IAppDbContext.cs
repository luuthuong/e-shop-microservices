using Core.BaseDbContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Interface;

public interface IAppDbContext: IBaseDbContext
{
    DbSet<Product> Product { get; set; }
    DbSet<User> User { get; set; }
    DbSet<Category> Category { get; set; }
}