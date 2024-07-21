namespace Core.Test;

public sealed class DummyCommandHandler(IRepository<DummyAgreegateRoot, DummyAggregateId> repository) : ICommandHandler<DummyCommand, DummyAgreegateRoot>
{
    public Task<DummyAgreegateRoot> Handle(DummyCommand request, CancellationToken cancellationToken)
    {
        return repository.InsertAsync(
            DummyAgreegateRoot.Create(),
            cancellationToken
        );
    }
}