
using Core.CQRS.Command;

namespace Application.Customers.Commands;

public class CreateCustomer: ICommand
{
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string PasswordConfirm { get; private set; }
    public string Name { get; private set; }
    public string ShippingAddress { get; private set; }

    public static CreateCustomer Create(
        string email,
        string password,
        string passwordConfirm,
        string name,
        string shippingAddress)
    {        
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email));
        
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));
        
        if (string.IsNullOrEmpty(passwordConfirm))
            throw new ArgumentNullException(nameof(passwordConfirm));
        
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));
        
        if (string.IsNullOrEmpty(shippingAddress))
            throw new ArgumentNullException(nameof(shippingAddress));

        return new (email, 
            password, 
            passwordConfirm, 
            name, 
            shippingAddress);
    }

    private CreateCustomer(
        string email,
        string password,
        string passwordConfirm,
        string name,
        string shippingAddress)
    {
        Email = email;
        Password = password;
        PasswordConfirm = passwordConfirm;
        Name = name;
        ShippingAddress = shippingAddress;
    }
}