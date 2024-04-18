using API.Requests.Payments;
using Application.Payments.CancellationPayment;
using Application.Payments.CreatePayment;
using Core.Api;
using Core.Infrastructure.Api;
using Domain;
using Domain.Payments;

namespace API.Endpoints;

public class PaymentEndpoints(IServiceScopeFactory serviceScopeFactory)
    : AbstractApiEndpoint(serviceScopeFactory), IApiEndpoint
{
    public void Register(IEndpointRouteBuilder app)
    {
        app.MapGet("/payments", (CreatePaymentRequest request) => ApiResponse(
            new CreatePaymentCommand(
                CustomerId.From(request.CustomerId),
                OrderId.From(request.OrderId),
                Money.From(
                    request.TotalAmount,
                    request.CurrencyCode
                ),
                Currency.FromCode(request.CurrencyCode)
            )
        ));

        app.MapDelete("/payments/{paymentId:guid}",
            (Guid paymentId, [AsParameters] CancellationPaymentRequest request) => ApiResponse(
                new CancellationPaymentCommand(
                    PaymentId.From(paymentId),
                    (PaymentCancelledReason)request.PaymentCancelReason
                )
            )
        );
    }
}