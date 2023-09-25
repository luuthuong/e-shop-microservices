namespace Domain.Core;

public record AuthResponse
{
    public bool IsAuthenticated { get; private set; }
    public string ErrorMsg { get; private set; }
    public string Msg { get; private set; }
    public string Token { get; private set; }

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