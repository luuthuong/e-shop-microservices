using Core.Infrastructure.Api;
using ProductSyncService.Application.Quotes.Commands.ConfirmQuote;

namespace API.Endpoints.Quotes;

public class ConfirmQuote(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory)
{
    public override void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/quotes/{quoteId}/confirm", (Guid quoteId) => ApiResponse(new ConfirmQuoteCommand(quoteId)) );
    }
}