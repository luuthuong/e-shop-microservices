using Application.Commands;
using Core.Api;
using Core.Infrastructure.Api;

namespace API.Endpoints;

internal sealed class CreateCustomer
{
    private record Request(
        string Name,
        string Email,
        string Password,
        string PasswordConfirm,
        string ShippingAddress,
        decimal CreditLimit
    );
    
    private class Endpoint(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
    {
        public void Register(IEndpointRouteBuilder app)
        {
            app.MapPost("/customers", (Request request) => ApiResponse(
                    new CreateCustomerCommand(
                        request.Email,
                        request.PasswordConfirm,
                        request.Password,
                        request.Name,
                        request.ShippingAddress,
                        request.CreditLimit
                    )
                )
            );
        }
    }
}