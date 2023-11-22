using Core.EF;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Interfaces;

public interface IAppDbContext: IDbContext
{
    DbSet<PaymentMethod>  PaymentMethod { get; set; }
    DbSet<PaymentOption> PaymentOption { get; set; }
}