using IdentityModel.Client;

namespace Identity;


public sealed record LoginFailure(
    ResponseErrorType ErrorType, 
    string? Description, 
    string? HttpErrorReason
);


public sealed record UserLoginResponse(
    string AccessToken,
    string RefreshToken,
    string Scope,
    LoginFailure Error
);

public sealed record UserInformationResponse(
    string UserName,
    string Email,
    string Scope,
    string Role
);