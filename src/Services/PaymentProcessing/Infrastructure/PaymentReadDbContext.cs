using Microsoft.EntityFrameworkCore;
using PaymentProcessing.Infrastructure.Models;

namespace PaymentProcessing.Infrastructure;

public class PaymentReadDbContext(DbContextOptions<PaymentReadDbContext> options) : DbContext(options)
{
    public DbSet<PaymentReadModel> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}