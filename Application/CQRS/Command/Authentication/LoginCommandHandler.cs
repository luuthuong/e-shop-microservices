using System.IdentityModel.Tokens.Jwt;
using Application.Helpers;
using AutoMapper;
using Domain.Core;
using Domain.Entities;
using Infrastructure.Database.Interface;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Command.Authentication;

public record LoginCommand(AuthRequest Request) : BaseRequest<AuthResponse>;

internal sealed class LoginCommandHandler: BaseRequestHandler<LoginCommand, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly JwtHandler _jwtHandler;

    public LoginCommandHandler(IMapper mapper, IAppDbContext dbContext, UserManager<User> userManager,
        JwtHandler jwtHandler) : base(mapper, dbContext)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _jwtHandler = jwtHandler ?? throw new ArgumentNullException(nameof(jwtHandler));
    }

    public override async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Request.UserName);
        if (user is null)
            return AuthResponse.Unauthorized("Invalid Authentication");
        
        var pwdValid = await _userManager.CheckPasswordAsync(user, request.Request.Password);
        if (!pwdValid)
            AuthResponse.Unauthorized("Invalid Authentication");

        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var claims = _jwtHandler.GetClaims(user);
        var tokenOptions = _jwtHandler.GenerateTokenSecurity(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return AuthResponse.Authorized(
            token,
            "User Authenticated"
        );
    }
}


