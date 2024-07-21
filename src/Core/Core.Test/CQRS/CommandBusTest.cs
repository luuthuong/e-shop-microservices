using Core.CQRS.Command;
using Core.Infrastructure.CQRS;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Test;

public class CommandBusTest
{
    readonly Mock<IMediator> _mediator = new Mock<IMediator>();
    readonly Mock<ILogger<CommandBus>> _commandBusLogger = new Mock<ILogger<CommandBus>>();

    [Test]
    public void SendCommand_ShouldReturnTrue()
    {

        //Arrange
            ICommandBus commandBus = new CommandBus(_mediator.Object, _commandBusLogger.Object);
            var repository = new Mock<IRepository<DummyAgreegateRoot, DummyAggregateId>>();
            DummyCommand command = new();
            DummyCommandHandler commandHandler = new(repository.Object);
        //Act
            var commandHandle = commandHandler.Handle(command, new CancellationToken());
        //Assert
            Assert.DoesNotThrowAsync(async () => await commandHandle);
    }
}
