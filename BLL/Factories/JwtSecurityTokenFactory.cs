using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BLL.Configurations;
using BLL.Interfaces;
using DAL.Entities;

namespace BLL.Factories
{
    public class JwtSecurityTokenFactory: IJwtTokenFactory
    {
        private readonly JwtTokenConfiguration jwtTokenConfiguration;

        public JwtSecurityTokenFactory(JwtTokenConfiguration jwtTokenConfiguration) => 
            this.jwtTokenConfiguration = jwtTokenConfiguration;

        private static List<Claim> GetClaims(User user) => new()
        {
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Authentication, user.Id)
        };

        public JwtSecurityToken BuildToken(User user) =>
            new(
                issuer: jwtTokenConfiguration.Issuer,
                audience: jwtTokenConfiguration.Audience,
                claims: GetClaims(user),
                expires: JwtTokenConfiguration.ExpirationDate,
                signingCredentials: jwtTokenConfiguration.Credentials
                );
    }
}
