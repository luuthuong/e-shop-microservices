using API.Requests;
using Application.Commands;
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
                    request.PasswordConfirm,
                    request.Password,
                    request.Name,
                    request.ShippingAddress,
                    request.CreditLimit
                )
            )
        );

        app.MapGet("/user-information", (Core.Identity.ITokenService tokenService) =>{
            
        }); 
    }
}