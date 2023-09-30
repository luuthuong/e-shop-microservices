namespace Core.BaseDTO;

public record AuthResponse
{
    public bool IsAuthenticated { get; private set; }
    public string ErrorMsg { get; private set; } = string.Empty;
    public string Msg { get; private set; } = string.Empty;
    public string Token { get; private set; } = string.Empty;

    public static AuthResponse Unauthorized(string msg = "") => new AuthResponse()
    {
        IsAuthenticated = false,
        ErrorMsg = msg
    };

    public static AuthResponse Authorized(string token, string msg = "") => new AuthResponse()
    {
        IsAuthenticated = true,
        Msg = msg,
        Token = token
    };

}