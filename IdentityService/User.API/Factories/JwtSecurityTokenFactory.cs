using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using User.API.Configurations;
using User.API.Interfaces;

namespace User.API.Factories
{
    public class JwtSecurityTokenFactory: IJwtSecurityTokenFactory
    {
        private readonly JwtTokenConfiguration jwtTokenConfiguration;

        public JwtSecurityTokenFactory(JwtTokenConfiguration jwtTokenConfiguration)
        {
            this.jwtTokenConfiguration = jwtTokenConfiguration;
        }

        public JwtSecurityToken BuildToken(IdentityUser user) =>
            new(
                issuer: jwtTokenConfiguration.Issuer,
                audience: jwtTokenConfiguration.Audience,
                claims: GetClaims(user),
                expires: JwtTokenConfiguration.ExpirationDate,
                signingCredentials: jwtTokenConfiguration.Credentials);

        private static List<Claim> GetClaims(IdentityUser user) => new()
        {
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Authentication, user.Id)
        };
    }
}
