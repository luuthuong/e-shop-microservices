using Core.CQRS.Command;
using Domain.Commands;

namespace Application.Payments.RecordingPayment;

public class RecordPaymentHandler : ICommandHandler<RecordPayment>
{
    public async Task Handle(RecordPayment command, CancellationToken cancellationToken)
    { }
}