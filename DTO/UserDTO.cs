namespace Domain.DTO;

public class UserDTO: BaseDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
}

public record AddUserRequest(string UserName, string Email, string Password, string DisplayName);