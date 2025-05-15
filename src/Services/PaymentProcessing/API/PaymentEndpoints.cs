using Core.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using PaymentProcessing.Application.Commands.ProcessingPayment;
using PaymentProcessing.Application.Commands.ProcessPayment;
using PaymentProcessing.Application.DTOs;
using PaymentProcessing.Application.Queries.GetPaymentByOrderId;

namespace PaymentProcessing.API;

public class PaymentEndpoints(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory)
{
    public override string GroupName => "Payments";

    public override void Register(IEndpointRouteBuilder route)
    {
        route.MapPost("/Process", ([FromBody] ProcessPaymentCommand request) => ApiResponse(request))
            .WithName("ProcessPayment")
            .Produces<bool>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(GroupName);

        route.MapGet("/getByOrderId/{id:guid}", ([FromRoute] Guid id) => ApiResponse(new GetPaymentByOrderIdQuery(id)))
            .WithName("GetPayment")
            .Produces<PaymentDTO>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(GroupName);
    }
}