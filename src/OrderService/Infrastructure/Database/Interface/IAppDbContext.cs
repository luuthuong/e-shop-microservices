using Domain.Entities;
using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Database.Interface;

public interface IAppDbContext
{
    DbSet<Product> Product { get; set; }
    DbSet<User> User { get; set; }
    DbSet<Category> Category { get; set; }
    DbSet<OutboxMessage> OutboxMessage { get; set; }
    ValueTask<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    DatabaseFacade Database { get; }
}