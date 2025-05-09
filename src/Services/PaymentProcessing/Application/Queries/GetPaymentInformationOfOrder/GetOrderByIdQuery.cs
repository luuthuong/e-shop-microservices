using MediatR;
using PaymentProcessing.Application.DTOs;

namespace PaymentProcessing.Application.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<PaymentDetailDto>
{
    public Guid OrderId { get; set; }
}