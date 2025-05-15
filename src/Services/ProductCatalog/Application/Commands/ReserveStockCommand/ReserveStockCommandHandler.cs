using Core.Domain;
using Core.Exception;
using MediatR;
using ProductCatalog.Domain.Product;
using ProductCatalog.Domain.Product.Events;
using ProductCatalog.Domain.Product.IntegrationEvents;

namespace ProductCatalog.Application.Commands.ReserveStockCommand;

public class ReserveStockCommandHandler(
    IEventStore<ProductAggregate> eventStore,
    ILogger<ReserveStockCommandHandler> logger
)
    : IRequestHandler<ReserveStockCommand, ReserveStockResult>
{
    public async Task<ReserveStockResult> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling ReserveStockCommand for OrderId: {OrderId}", request.OrderId);

        var productIds = request.Items.Select(i => i.ProductId).ToList();

        var products = (await eventStore.LoadAsync(productIds)).ToList();

        if (!products.Any())
        {
            logger.LogWarning("No products found for OrderId: {OrderId}", request.OrderId);
            throw new NotFoundException($"Products with ids {string.Join(", ", productIds)} not found");
        }

        var productDict = products.ToDictionary(p => p.Id);

        ReserveStockResult reserveResult = new(true, []);

        foreach (var item in request.Items)
        {
            if (!productDict.TryGetValue(item.ProductId, out var product))
            {
                logger.LogWarning("Product with id {ProductId} not found for OrderId: {OrderId}", item.ProductId,
                    request.OrderId);
                reserveResult = reserveResult with { AllStockReserved = false };
                continue;
            }

            var isReserved = product.ReserveStock(request.OrderId, item.Quantity);

            if (!isReserved & reserveResult.AllStockReserved)
            {
                reserveResult = reserveResult with { AllStockReserved = false };
            }

            reserveResult.Items.Add(new(item.ProductId, isReserved, item.Quantity));
        }

        if (reserveResult.AllStockReserved)
        {
            await eventStore.AppendToOutboxAsync(IntegrationEvent.Create(new OrderStockReserved(request.OrderId)));
            await eventStore.SaveAsync(productDict.Values);
        }
        else
        {
            var outOfStockEvents = productDict.Values.SelectMany(p => p.GetUncommittedEvents())
                .OfType<StockReservedEvent>()
                .ToList();

            if (outOfStockEvents.Count == 0)
            {
                logger.LogWarning("No stock available for OrderId: {OrderId}", request.OrderId);
            }

            await eventStore.AppendToOutboxAsync(
                IntegrationEvent.Create(new OrderOutOfStock(request.OrderId,
                    outOfStockEvents.Select(p => p.AggregateId)))
            );
            
            await eventStore.SaveAsync(products);
        }
        
        return reserveResult;
    }
}