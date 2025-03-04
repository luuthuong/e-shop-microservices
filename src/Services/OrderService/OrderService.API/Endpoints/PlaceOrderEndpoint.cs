using Application.Orders.PlacingOrder;
using Core.Infrastructure.Api;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

record PlaceOrderRequest(Guid CustomerId, Guid QuoteId);

class PlaceOrderEndpoint(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory)
{
    public override void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/place",
            ([FromBody]PlaceOrderRequest request) =>
                ApiResponse(
                    PlaceOrder.Create(
                        CustomerId.From(request.CustomerId),
                        QuoteId.From(request.QuoteId)
                    )
                )
        );
    }
}