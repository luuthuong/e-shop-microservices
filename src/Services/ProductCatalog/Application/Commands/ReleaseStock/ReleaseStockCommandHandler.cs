using Core.Domain;
using Core.Exception;
using MediatR;
using ProductCatalog.Domain.Product;

namespace ProductCatalog.Application.Commands.ReleaseStock;

public class ReleaseStockCommandHandler(IEventStore<ProductAggregate> eventStore): IRequestHandler<ReleaseStockCommand, bool>
{
    public async Task<bool> Handle(ReleaseStockCommand request, CancellationToken cancellationToken)
    {
        var product = await eventStore.LoadAsync(request.ProductId);
        
        if (product == null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }
        
        product.ReleaseStock(request.OrderId, request.Quantity);
        
        await eventStore.SaveAsync(product);
        
        return true;
    }
}