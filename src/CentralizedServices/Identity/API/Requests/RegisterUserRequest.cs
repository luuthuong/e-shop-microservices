namespace Identity.API.Requests;

public record RegisterUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PasswordConfirm { get; set; }
}