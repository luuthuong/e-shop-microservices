namespace Ordering.Infrastructure.Models;

public class OrderHistoryReadModel
{
    public Guid OrderId { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string Status { get; private set; }
    public string Description { get; private set; }

    private OrderHistoryReadModel()
    {
    }

    public static OrderHistoryReadModel Create(Guid orderId, string status, string description, DateTime timestamp)
    {
        return new OrderHistoryReadModel
        {
            OrderId = orderId,
            Timestamp = timestamp,
            Status = status,
            Description = description
        };
    }
}