using Core.EF;
using Microsoft.EntityFrameworkCore;
using PaymentProcessing.Infrastructure.Models;

namespace PaymentProcessing.Infrastructure;

public class PaymentReadDbContext(DbContextOptions<PaymentReadDbContext> options) : DbContext(options)
{
    public DbSet<PaymentReadModel> Payments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentReadDbContext).Assembly);
    }
}