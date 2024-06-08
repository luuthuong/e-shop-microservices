using Core.CQRS.Command;

namespace Core.Test;

public sealed class DummyCommandHandler(EF.IRepository<DummyAgreegateRoot, DummyAggregateId> repository) : ICommandHandler<DummyCommand, DummyAgreegateRoot>
{
    public Task<DummyAgreegateRoot> Handle(DummyCommand request, CancellationToken cancellationToken)
    {
        return repository.InsertAsync(
            DummyAgreegateRoot.Create()
        );
    }
}