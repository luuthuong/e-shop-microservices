using System.Security.Claims;
using Core;
using IdentityModel;

namespace Identity.Domains;

public static class UserExtension
{
    public static IEnumerable<Claim> GetUserAdminClaims(this User user)
    {
        yield return new(JwtClaimTypes.Name, user.UserName!);
        yield return new(JwtClaimTypes.Email, user.Email!);
        yield return new(JwtClaimTypes.Role, RoleConstants.Admin);
    }


    public static IEnumerable<Claim> GetUserCustomerClaims(this User user)
    {
        yield return new(JwtClaimTypes.Name, user.UserName!);
        yield return new(JwtClaimTypes.Email, user.Email!);
        yield return new(JwtClaimTypes.Role, RoleConstants.Customer);
    }
}