using Core.CQRS.Command;
using Domain;
using Domain.Payments;

namespace Application.Payments.CreatePayment;

public sealed record CreatePaymentCommand(
    CustomerId CustomerId,
    OrderId OrderId,
    Money TotalAmount,
    Currency Currency
) : ICommand;