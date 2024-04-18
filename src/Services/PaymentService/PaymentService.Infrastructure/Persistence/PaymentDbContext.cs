using Core.Infrastructure.EF.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class PaymentDbContext(DbContextOptions options) : BaseDbContext(options)
{

}