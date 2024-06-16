using Core.CQRS.Query;
using Core.Http;
using Core.Identity;
using Core.Infrastructure;
using CustomerService.DTO.Customers;
using CustomerService.Infrastructure.Configs;
using Microsoft.Extensions.Options;

namespace Application.Queries;

internal sealed class GetCustomerLoginViaTokenQueryHandler(
    IOptions<AppSettings> appSettings,
    IHttpRequest httpRequest,
    ITokenService tokenService
) : IQueryHandler<GetCustomerLoginViaTokenQuery, CustomerLoginDTO?>
{
    public async Task<CustomerLoginDTO?> Handle(GetCustomerLoginViaTokenQuery request,
        CancellationToken cancellationToken)
    {
        var uri = appSettings.Value.TokenIssuerSettings.GetIdentityUserInfoUrl();
        var token = await tokenService.GetUserTokenFromHttpContextAsync();

        if (string.IsNullOrEmpty(token))
            return null;
        return await httpRequest.GetAsync<CustomerLoginDTO>(uri, token);
    }
}