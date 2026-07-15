using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Authentication.Interfaces;

namespace WMS.Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Dictionary<string, string>> GetUsersAsync(IEnumerable<string> userIds)
        {
            var ids = userIds
                .Distinct()
                .ToList();

            var users = await _userManager.Users
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            return users.ToDictionary(
                x => x.Id,
                x => x.Email ?? "Unknown");
        }
    }
}