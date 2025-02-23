using Core.CQRS.Command;
using Core.EF;
using Core.EventBus;
using Core.Http;
using Core.Identity;
using Core.Integration;
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Orders.PlacingOrder;

public class PlaceOrderHandler(
    ILogger<PlaceOrderHandler> logger,
    IOrderRepository orderRepository,
    IEventOutboxRepository eventOutboxRepository,
    IUnitOfWork unitOfWork,
    IOptions<IntegrationHttpSetting> integrationHttpSettingOptions,
    IOptions<TokenIssuerSettings> tokenIssuerSettingsOptions,
    ITokenService tokenService,
    IHttpRequest httpRequest
) : ICommandHandler<PlaceOrder>
{
    private readonly IntegrationHttpSetting _integrationHttpSetting = integrationHttpSettingOptions.Value;
    
    public async Task Handle(PlaceOrder command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Placing Order, CustomerId: {command.CustomerId.Value}, QuoteId: {command.QuoteId.Value}...");

        var appToken = await tokenService.GetApplicationTokenAsync(tokenIssuerSettingsOptions.Value);

        if (appToken is null)
        {
            throw new ArgumentNullException(nameof(appToken));
        }

        if (appToken.AccessToken != null)
            await httpRequest.GetAsync<object>(_integrationHttpSetting.ApiGatewayURL, appToken.AccessToken);

        var order = Order.Create(
            new(command.CustomerId, command.QuoteId)
        );

        await orderRepository.InsertAsync(order, cancellationToken);

        var @events = order.GetDomainEvents();

        var outboxEvents = @events.Select(IntegrationEvent.FromDomainEvent);

        await eventOutboxRepository.InsertAsync(outboxEvents, cancellationToken);

        await unitOfWork.SaveChangeAsync(cancellationToken);
        
        logger.LogInformation("Order placed.");
    }

    private Task ConfirmQuote(Guid quoteId, CancellationToken cancellationToken)
    {
        // var apiRoute = 
        return Task.CompletedTask;
    }
}
