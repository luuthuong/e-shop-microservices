using System.Data;
using Core.BaseDomain;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class User: IdentityUser<Guid>, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public override string UserName { get; set; }
    public string DisplayName { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime UpdatedDate { get; private set; }
    public User(){}

    private User(string userName, string email, string password, string displayName) =>
        (UserName, Email, DisplayName, CreatedDate) = (userName, email, displayName ?? string.Empty, DateTime.Now);

    public static User Create(string userName, string email, string password, string displayName)
    {
        if (string.IsNullOrEmpty(userName))
            throw new NoNullAllowedException($"{nameof(userName)} can't null.");
        
        if(string.IsNullOrEmpty(email))
            throw new NoNullAllowedException($"{nameof(email)} can't null.");

        if (string.IsNullOrEmpty(password))
            throw new NoNullAllowedException($"{nameof(password)} can't null.");
        return new User(userName, email, password, displayName);
    }

    public void Update(string displayName, string email)
    {
        DisplayName = displayName;
        Email = email;
        UpdatedDate = DateTime.Now;
    }

    public void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();
}