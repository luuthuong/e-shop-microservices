using API.Requests;
using Application.Customers.Commands;
using Core.Api;
using Core.CQRS.Command;

namespace API.Endpoints;

internal sealed class CustomerEndpoints : IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/customers", async (ICommandBus commandBus, CreateCustomerRequest request) =>
        {
            await commandBus.SendAsync(
                CreateCustomer.Create(
                    request.Email,
                    request.Password,
                    request.PasswordConfirm,
                    request.Name,
                    request.ShippingAddress
                )
            );
        });
    }
}