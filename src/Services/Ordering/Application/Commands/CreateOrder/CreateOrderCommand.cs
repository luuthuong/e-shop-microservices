using MediatR;

namespace Ordering.Application.Commands;

public sealed record CreateOrderCommand(
    Guid CustomerId,
    List<OrderItemCommand> Items,
    ShippingAddressCommand ShippingAddress,
    string CustomerEmail,
    string CustomerPhone,
    string Currency = "USD"
) : IRequest<CreateOrderResponse>;

public sealed record OrderItemCommand(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal Price
);

public record ShippingAddressCommand(
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    string RecipientName,
    string PhoneNumber
);