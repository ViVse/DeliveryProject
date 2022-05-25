using DAL.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace BLL.Interfaces
{
    public interface IJwtTokenFactory
    {
        JwtSecurityToken BuildToken(User user);
    }
}
