namespace OrderManagement.Application.Commands;

public record CreateOrderResponse(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    string Currency,
    string Status,
    DateTime OrderDate
);