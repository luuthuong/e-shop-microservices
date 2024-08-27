using Core.Infrastructure.EF.DbContext;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence;

public class CustomerDbContext(DbContextOptions options) : BaseDbContext(options)
{
    public DbSet<Customer>? Customer { get; set; }
}