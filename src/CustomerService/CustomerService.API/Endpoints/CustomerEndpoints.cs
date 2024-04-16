using API.Requests.Customers;
using Application.Commands.Customers;
using Core.Api;
using Core.Infrastructure.Api;

namespace API.Endpoints;

public class CustomerEndpoints(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/customers", (CreateCustomerRequest request) => ApiResponse(
                new CreateCustomerCommand(
                    request.Email,
                    request.Password,
                    request.PasswordConfirm,
                    request.Name,
                    request.ShippingAddress,
                    request.CreditLimit
                )
            )
        );
    }
}