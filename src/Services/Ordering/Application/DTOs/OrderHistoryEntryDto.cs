using Ordering.Infrastructure.Models;

namespace Ordering.Application.DTOs;

public class OrderHistoryEntryDto
{
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }

    public static OrderHistoryEntryDto FromOrderHistory(OrderHistoryReadModel orderHistory)
    {
        return new()
        {
            Timestamp = orderHistory.Timestamp,
            Status = orderHistory.Status,
            Description = orderHistory.Description
        };
    }
}