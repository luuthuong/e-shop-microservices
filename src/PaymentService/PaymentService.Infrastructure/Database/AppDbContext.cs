using Core.BaseDbContext;
using Domain.Entities;
using Infrastructure.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class AppDbContext: BaseDbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<PaymentMethod>? PaymentMethod { get; set; }
    public DbSet<PaymentOption>? PaymentOption { get; set; }
}