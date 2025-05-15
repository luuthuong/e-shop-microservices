using MediatR;

namespace ProductCatalog.Application.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    int AvailableStock,
    string Category)
    : IRequest<Guid>;