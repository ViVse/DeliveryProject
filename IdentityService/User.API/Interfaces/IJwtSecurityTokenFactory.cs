using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace User.API.Interfaces
{
    public interface IJwtSecurityTokenFactory
    {
        JwtSecurityToken BuildToken(IdentityUser user);
    }
}
