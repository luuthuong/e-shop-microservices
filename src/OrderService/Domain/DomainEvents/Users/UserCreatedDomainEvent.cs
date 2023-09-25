using Domain.Base;
using Domain.Entities;

namespace Domain.DomainEvents.Users;

public record UserCreatedDomainEvent(User User): IDomainEvent;