using Core.Domain;
using Core.Exception;
using MediatR;
using ProductCatalog.Domain.Product;

namespace ProductCatalog.Application.Commands.UpdateProductCommand;

public class UpdateProductCommandHandler(IEventStore<ProductAggregate> eventStore)
    : IRequestHandler<UpdateProductCommand, bool>
{
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await eventStore.LoadAsync(request.ProductId);
        if (product == null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found");
        }

        product.UpdateDetails(
            request.Name,
            request.Description,
            request.Price,
            request.PictureUrl,
            request.Category
        );

        try
        {
            await eventStore.SaveAsync(product);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
}