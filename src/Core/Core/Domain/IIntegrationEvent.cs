using MediatR;

namespace Core.Domain;

public interface IIntegrationEvent: INotification
{
    Guid Id { get; } 
}
