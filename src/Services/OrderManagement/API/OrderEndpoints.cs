using Core.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Queries.GetOrderById;
using OrderManagement.Application.Queries.GetOrdersByCustomer;
using OrderManagement.Infrastructure.Models;

namespace OrderManagement.API;

public class OrderEndpoints(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory)
{
    public override string GroupName => "Orders";

    public override void Register(IEndpointRouteBuilder route)
    {
        route.MapPost("/", ([FromBody] CreateOrderCommand request) => ApiResponse(request)).WithName("CreateOrder")
            .Produces<bool>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(GroupName);

        route.MapPost("/submit/{id:guid}",
                ([FromRoute] Guid id) => ApiResponse(new SubmitOrderCommand() { OrderId = id }))
            .WithName("SubmitOrder")
            .Produces<bool>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(GroupName);

        route.MapGet("/{id:guid}", ([FromRoute] Guid id) => ApiResponse(
                new GetOrderByIdQuery()
                {
                    OrderId = id
                })
            )
            .WithName("GetOrder")
            .Produces<OrderReadModel>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(GroupName);

        route.MapGet("/customer:{customerId:guid}", ([FromRoute] Guid customerId) => ApiResponse(
                new GetOrdersByCustomerQuery()
                {
                    CustomerId = customerId
                })
            )
            .WithName("GetOrdersByCustomer")
            .Produces<List<OrderReadModel>>()
            .WithTags(GroupName);
    }
}