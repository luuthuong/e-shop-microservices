using Core.CQRS.Query;

namespace Application.Orders.GettingOrders;

public class GetOrdersHandler() : IQueryHandler<GetOrders, IList<OrderViewModel>>
{

    public Task<IList<OrderViewModel>> Handle(GetOrders query, CancellationToken cancellationToken)
    {
        return default;
    }
}