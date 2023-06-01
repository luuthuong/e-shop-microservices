using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Database;

public class AppDbContext: DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = Extensions.GetDbConnection();
        optionsBuilder.UseSqlServer(connectionString);
    }
}