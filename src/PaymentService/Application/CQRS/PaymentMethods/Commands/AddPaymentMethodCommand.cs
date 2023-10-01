using Application.DTOs;
using Application.Helpers;
using AutoMapper;
using Core.Mediator;
using Domain.Entities;
using Infrastructure.Database.Interfaces;

namespace Application.CQRS.PaymentMethods.Commands;

public record AddPaymentMethodCommand(AddPaymentMethodRequest Request): IBaseRequest<AddPaymentMethodResponse>;

internal sealed class AddPaymentMethodCommandHandler: BaseRequestHandler<AddPaymentMethodCommand,AddPaymentMethodResponse>
{
    public AddPaymentMethodCommandHandler(IMapper mapper, IAppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public override async Task<AddPaymentMethodResponse> Handle(AddPaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var entity = PaymentMethod.Create(request.Request.Name, request.Request.Description);
        await DBContext.PaymentMethod.AddAsync(entity, cancellationToken);
        return new()
        {
            Success = true,
            Data = Mapper.Map<PaymentMethod, PaymentMethodDTO>(entity)
        };
    }
}