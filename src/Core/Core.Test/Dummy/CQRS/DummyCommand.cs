using Core.CQRS.Command;

namespace Core.Test;

public record DummyCommand: ICommand<DummyAgreegateRoot>;