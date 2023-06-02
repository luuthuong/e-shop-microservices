using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Database.Interface;

public interface IAppDbContext
{
    DbSet<Product> Product { get; set; }
    DbSet<User> User { get; set; }
    DbSet<Category> Category { get; set; }
    ValueTask<int> SaveChangeAsync(CancellationToken cancellationToken = default);
}