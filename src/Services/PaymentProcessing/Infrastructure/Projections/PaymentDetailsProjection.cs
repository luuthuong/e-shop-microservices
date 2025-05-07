using Core.Infrastructure.EF.Projections;
using PaymentProcessing.Infrastructure.Models;

namespace PaymentProcessing.Infrastructure.Projections;

public class PaymentDetailsProjection : StreamProjection<PaymentReadModel, PaymentReadDbContext>
{
    public PaymentDetailsProjection(PaymentReadDbContext dbContext) : base(dbContext)
    {
    }
}