using Application.Customers;
using Core.Api;
using Core.Infrastructure.Api;
using Domain;

namespace API.Endpoints;

public class UpdateCustomer(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
{
    private record Request
    {
        public string Email { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string ShippingAddress { get; init; } = default!;
        public decimal CreditLimit { get; init; }
    }

    public override void Register(IEndpointRouteBuilder app)
    {
        app.MapPut("/customers/{id}", (Guid id, Request request) => ApiResponse(
                new UpdateCustomerCommand(
                    CustomerId.From(id),
                    request.Email,
                    request.Name,
                    request.CreditLimit
                )
            )
        );
    }
}