using MediatR;

namespace ProductCatalog.Application.Commands.ReleaseStock;

public record ReleaseStockCommand(
    Guid ProductId,
    Guid OrderId,
    int Quantity
) : IRequest<bool>;