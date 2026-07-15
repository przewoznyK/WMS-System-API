using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WMS.Client.Services.Interfaces;
using WMS.Domain.Enums;

namespace WMS.Client.Services.Implementations
{
    public class JwtTokenService : IJwtTokenService
    {
        public string? GetEmail(string token)
        {
            return GetClaim(token, JwtRegisteredClaimNames.Email);
        }

        public UserRoleType? GetRole(string token)
        {
            var role = GetClaim(token, ClaimTypes.Role);

            return Enum.TryParse<UserRoleType>(role, out var userRole) ? userRole : null;
        }

        public string? GetClaim(string token, string claimType)
        {
            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

                return jwt.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
            }
            catch
            {
                return null;
            }
        }

        public bool IsExpired(string token)
        {
            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return jwt.ValidTo <= DateTime.UtcNow;
            }
            catch
            {
                return true;
            }
        }
    }
}