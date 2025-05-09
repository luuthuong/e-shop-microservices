using MediatR;

namespace ProductCatalog.Application.Commands.ReserveStockCommand;

public record ReserveStockCommand(Guid ProductId, Guid OrderId, int Quantity) : IRequest<bool>;