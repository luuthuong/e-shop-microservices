using Core.Domain;
using Core.Exception;
using Core.Infrastructure.Utils;
using MediatR;
using ProductCatalog.Domain.Product;
using ProductCatalog.Domain.Product.Events;

namespace ProductCatalog.Application.Commands.ReserveStockCommand;

public class ReserveStockCommandHandler(IEventStore<ProductAggregate> eventStore)
    : IRequestHandler<ReserveStockCommand, bool>
{
    public async Task<bool> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var product = await eventStore.LoadAsync(request.ProductId);

        if (product == null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found");
        }

        product.ReserveStock(request.OrderId, request.Quantity);

        var stockReservedEvent = product.GetUncommittedEvents().FirstOrDefaultOfType<StockReservedEvent>();

        if (stockReservedEvent == null)
        {
            throw new InvalidOperationException("No stock reserved event found");
        }

        await eventStore.AppendToOutboxAsync(stockReservedEvent);
        await eventStore.SaveAsync(product);

        return true;
    }
}