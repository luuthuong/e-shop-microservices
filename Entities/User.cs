using System.Data;

namespace Domain.Entities;

public class User: BaseEntity
{
    public string UserName { get; private set; }
    public string DisplayName { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; }
    
    public User(){}

    private User(string userName, string email, string password, string displayName) =>
        (UserName, Email, Password, DisplayName, CreatedDate) = (userName, email, password, displayName ?? string.Empty, DateTime.Now);

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
}