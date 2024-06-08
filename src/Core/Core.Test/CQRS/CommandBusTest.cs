using Core.CQRS.Command;
using Core.Infrastructure.CQRS;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Test;

public class CommandBusTest
{
    Mock<IMediator> mediator = new Mock<IMediator>();
    Mock<ILogger<CommandBus>> commandBusLogger = new Mock<ILogger<CommandBus>>();

    [Test]
    public void SendCommand_ShouldReturnTrue()
    {

        //Arrange
            ICommandBus commandBus = new CommandBus(mediator.Object, commandBusLogger.Object);
            var repository = new Mock<EF.IRepository<DummyAgreegateRoot, DummyAggregateId>>();
            DummyCommand command = new();
            DummyCommandHandler commandHandler = new(repository.Object);
        //Act
            var commandHandle = commandHandler.Handle(command, new CancellationToken());
        //Assert
            Assert.DoesNotThrowAsync(async () => await commandHandle);
    }
}
