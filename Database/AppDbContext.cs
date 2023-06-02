using Domain.Database.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Database;

public class AppDbContext: DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Product { get; set; }
    
    public DbSet<User> User { get; set; }
    public DbSet<Category> Category { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = Extensions.GetDbConnection();
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
    public async ValueTask<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return await this.SaveChangesAsync(cancellationToken);
    }
}