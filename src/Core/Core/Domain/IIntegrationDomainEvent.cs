using MediatR;

namespace Core.Domain;

public interface IIntegrationDomainEvent: INotification
{
    Guid Id { get; }
}