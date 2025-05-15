using Core.Domain;
using MediatR;
using ProductCatalog.Domain.Product;

namespace ProductCatalog.Application.Commands.CreateProduct;

public class CreateProductCommandHandler(IEventStore<ProductAggregate> eventStore)
    : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = ProductAggregate.Create(request.Name, request.Description, request.Price, request.AvailableStock,
            request.PictureUrl, request.Category);

        await eventStore.SaveAsync(product);

        return product.Id;
    }
}