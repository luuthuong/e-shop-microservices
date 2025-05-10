using MediatR;

namespace ProductCatalog.Application.Commands.ReserveStockCommand;

public record ReserveStockCommand(Guid OrderId, IEnumerable<ReserveItem> Items) : IRequest<ReserveStockResult>;

public record ReserveItem(Guid ProductId, int Quantity);

public record ReserveStockResult(bool AllStockReserved, IList<ProductReservation> Items);

public record ProductReservation(Guid ProductId, bool IsReserved, int Quantity);