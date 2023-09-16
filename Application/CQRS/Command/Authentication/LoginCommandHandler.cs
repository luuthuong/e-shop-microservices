using System.IdentityModel.Tokens.Jwt;
using Core;
using Core.Mediator;
using Domain.Core;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Command.Authentication;

public record LoginCommand(AuthRequest Request) : BaseRequest<AuthResponse>;

internal sealed class LoginCommandHandler: BaseRequestHandler<LoginCommand, AuthResponse>
{

    private readonly UserManager<User> _userManager;
    private readonly JwtHandler _jwtHandler;

    public LoginCommandHandler(UserManager<User> userManager, JwtHandler jwtHandler)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _jwtHandler = jwtHandler ?? throw new ArgumentNullException(nameof(jwtHandler));
    }

    public override async Task<BaseResponse<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Request.UserName);
        if (user is null)
            return new BaseResponse<AuthResponse>()
            {
                Data = AuthResponse.Unauthorized("Invalid Authentication"),
                Success = false
            };
        
        var pwdValid = await _userManager.CheckPasswordAsync(user, request.Request.Password);
        if (!pwdValid)
            return new BaseResponse<AuthResponse>()
            {
                Data = AuthResponse.Unauthorized("Invalid Authentication"),
                Success = false
            };

        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var claims = _jwtHandler.GetClaims(user);
        var tokenOptions = _jwtHandler.GenerateTokenSecurity(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new BaseResponse<AuthResponse>()
        {
            Data = AuthResponse.Authorized(
                token,
                "User Authenticated"
            ),
            Success = true
        };
    }
}


