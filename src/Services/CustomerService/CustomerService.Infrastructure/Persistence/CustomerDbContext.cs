using Core.Infrastructure.EF.DbContext;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence;

public class CustomerDbContext: BaseDbContext
{
    
    public DbSet<Customer>? Customer { get; set; }
    
    public CustomerDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}