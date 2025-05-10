using Core.Exception;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentProcessing.Application.DTOs;
using PaymentProcessing.Infrastructure;

namespace PaymentProcessing.Application.Queries.GetPaymentByOrderId;

public class GetPaymentByOrderIdQueryHandler(PaymentReadDbContext dbContext)
    : IRequestHandler<GetPaymentByOrderIdQuery, PaymentDTO>
{
    public async Task<PaymentDTO> Handle(GetPaymentByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await dbContext.Payments
            .AsNoTracking()
            .Where(p => p.OrderId == request.OrderId)
            .FirstOrDefaultAsync(cancellationToken);

        if (payment == null)
        {
            throw new NotFoundException($"Payment with OrderId {request.OrderId} not found.");
        }
        
        return PaymentDTO.From(payment);
    }
}