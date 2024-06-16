using Core.CQRS.Command;
using Core.EF;
using Core.Exception;
using Core.Http;
using Core.Identity;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Commands;

internal sealed class CreateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    IHttpRequest httpRequest,
    IOptions<TokenIssuerSettings> tokenOptions
) : ICommandHandler<CreateCustomerCommand>
{
    private readonly TokenIssuerSettings _tokenIssuerSettings = tokenOptions.Value;

    public async Task Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            new(
                request.Name,
                request.Email,
                Address.From(request.ShippingAddress),
                CreditLimit.From(request.CreditLimit)
            )
        );
        
        await customerRepository.InsertAsync(customer, cancellationToken: cancellationToken);
        
        var identityResult = await CreateCustomerAccountLogin(request);

        if (identityResult is null)
            throw new DomainLogicException("An error occurred during create customer's account");
        
        if (!identityResult.Succeeded)
            throw new DomainLogicException(identityResult.Errors.First().Description);
        
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }

    private async Task<IdentityResult?> CreateCustomerAccountLogin(CreateCustomerCommand command)
    {
        var identityUserCreateUrl = $"{_tokenIssuerSettings.Authority}/api/v1/accounts/register";
        return await httpRequest.PostAsync<RegisterUserRequest, IdentityResult>(
            identityUserCreateUrl,
            new RegisterUserRequest(command.Email, command.Password, command.PasswordConfirm)
        );
    }

    private record RegisterUserRequest(string Email, string Password, string PasswordConfirm);
}