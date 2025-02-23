using Core.CQRS.Query;

namespace Application.Orders.GettingOrderEventHistory;

public record OrderEventHistory() : IQuery<int>
{
   
}