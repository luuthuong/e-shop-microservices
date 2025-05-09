using Core.Infrastructure.Api;

namespace PaymentProcessing.API;

public class PaymentEndpoints(IServiceScopeFactory serviceScopeFactory) : AbstractApiEndpoint(serviceScopeFactory)
{
    public override string GroupName => "/orders";

    public override void Register(IEndpointRouteBuilder route)
    {
      
    }
}