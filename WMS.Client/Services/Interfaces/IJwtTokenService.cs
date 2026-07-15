using WMS.Domain.Enums;

namespace WMS.Client.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string? GetClaim(string token, string claimType);
        string? GetEmail(string token);
        UserRoleType? GetRole(string token);
        bool IsExpired(string token);

    }
}