using Core.Domain;
using Core.Exception;

namespace ProductCatalog.Domain.ValueObjects;

public class StockReservation : ValueObject<StockReservation>
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public DateTime ReservedAt { get; private set; }

    public StockReservation(Guid orderId, Guid productId, int quantity)
    {
        if (orderId == Guid.Empty)
            throw new DomainRuleException("Order ID cannot be empty");

        if (productId == Guid.Empty)
            throw new DomainRuleException("Product ID cannot be empty");

        if (quantity <= 0)
            throw new DomainRuleException("Quantity must be greater than zero");

        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        ReservedAt = DateTime.UtcNow;
    }

    protected override IEnumerable<object> EqualityComponents =>
    [
        OrderId,
        ProductId,
        Quantity,
        ReservedAt
    ];
}