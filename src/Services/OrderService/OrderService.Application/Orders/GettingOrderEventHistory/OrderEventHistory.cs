using Core.CQRS.Query;

namespace EcommerceDDD.OrderProcessing.Application.GettingOrderEventHistory;

public record OrderEventHistory() : IQuery<int>
{
   
}