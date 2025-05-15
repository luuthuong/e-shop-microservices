using MediatR;

namespace ProductCatalog.Application.Commands.UpdateProductCommand;

public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    int AvailableStock,
    string Category)
: IRequest<bool>;