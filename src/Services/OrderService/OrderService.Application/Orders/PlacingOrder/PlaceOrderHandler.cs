using Core.CQRS.Command;
using Domain.Commands;

namespace EcommerceDDD.OrderProcessing.Application.Orders.PlacingOrder;

public class PlaceOrderHandler : ICommandHandler<PlaceOrder>
{
    public async Task Handle(PlaceOrder command, CancellationToken cancellationToken)
    {}
}

public record QuoteViewModelResponse(
    Guid QuoteId,
    Guid CustomerId,
    List<QuoteItemViewModel> Items,
    string CurrencyCode,
    decimal TotalPrice);

public record class QuoteItemViewModel(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice);