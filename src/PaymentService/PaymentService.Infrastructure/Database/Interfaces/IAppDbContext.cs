using Core.BaseDbContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Interfaces;

public interface IAppDbContext: IBaseDbContext
{
    DbSet<PaymentMethod>  PaymentMethod { get; set; }
    DbSet<PaymentOption> PaymentOption { get; set; }
}