using Core.CQRS.Command;

namespace Application.Payments.CreatePayment;

internal sealed class CreatePaymentCommandHandler: ICommandHandler<CreatePaymentCommand>
{
    public Task Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}