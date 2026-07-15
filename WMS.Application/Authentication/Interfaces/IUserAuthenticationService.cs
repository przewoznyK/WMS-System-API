using WMS.Application.Authentication.Models;

namespace WMS.Application.Authentication.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<AuthUser?> FindUserAsync(string email);
        Task<bool> ValidatePasswordAsync(string email, string password);
        Task<IEnumerable<string>> GetRolesAsync(string email);
    }
}