using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public class JwtHandler
{
    private readonly IConfigurationSection _jwtSetting;

    public JwtHandler(IConfiguration configuration)
    {
        var configuration1 = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _jwtSetting = configuration1.GetSection("JWTSetting");
    }

    public SigningCredentials GetSigningCredentials()
    {
        string secureKey = _jwtSetting["securityKey"];
        if (string.IsNullOrEmpty(secureKey))
            throw new Exception($"{nameof(secureKey)} cannot empty or null");
        var key = Encoding.UTF8.GetBytes(secureKey);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(
            secret,
            SecurityAlgorithms.HmacSha256Signature
        );
    }

    public IList<Claim> GetClaims(User user)
    {
        if (string.IsNullOrEmpty(user.UserName))
            throw new ArgumentNullException(nameof(User.UserName));
        var claims = new List<Claim>()
        {
            CreateClaim(ClaimTypes.Name, user.UserName),
        };
        
        if(!string.IsNullOrEmpty(user.Email))
            claims.Add(CreateClaim(ClaimTypes.Email, user.Email));
        return claims;
    }

    private Claim CreateClaim(string type, string value) => new Claim(type, value);

    public JwtSecurityToken GenerateTokenSecurity(SigningCredentials signingCredentials, IEnumerable<Claim> claims) =>
        new(
            issuer: _jwtSetting["validIssuer"],
            audience: _jwtSetting["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddDays(Convert.ToDouble(_jwtSetting["expiryInHours"])),
            signingCredentials: signingCredentials
        );
}