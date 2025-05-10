using MediatR;
using PaymentProcessing.Application.DTOs;

namespace PaymentProcessing.Application.Queries.GetPaymentByOrderId;

public record GetPaymentByOrderIdQuery(Guid OrderId) : IRequest<PaymentDTO>;