using Core.CQRS.Query;
using Domain;
using EcommerceDDD.OrderProcessing.Application.GettingOrderEventHistory;

namespace Application.Orders.GettingOrderEventHistory;

public record class GetOrderEventHistory : IQuery<IList<OrderEventHistory>>
{
    public OrderId OrderId { get; private set; }

    public static GetOrderEventHistory Create(OrderId orderId)
    {
        if (orderId is null)
            throw new ArgumentNullException(nameof(orderId));

        return new GetOrderEventHistory(orderId);
    }

    private GetOrderEventHistory(OrderId orderId) => OrderId = orderId;
}