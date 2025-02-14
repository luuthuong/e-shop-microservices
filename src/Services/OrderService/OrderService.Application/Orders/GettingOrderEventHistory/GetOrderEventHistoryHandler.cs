using Core.CQRS.Query;
using EcommerceDDD.OrderProcessing.Application.GettingOrderEventHistory;

namespace Application.Orders.GettingOrderEventHistory;

public class GetOrderEventHistoryHandler : IQueryHandler<GetOrderEventHistory, IList<OrderEventHistory>> 
{
    public async Task<IList<OrderEventHistory>> Handle(GetOrderEventHistory query, 
        CancellationToken cancellationToken)
    {
        return default;
    }
}
