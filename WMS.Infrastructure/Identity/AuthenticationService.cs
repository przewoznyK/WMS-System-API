using Microsoft.AspNetCore.Identity;
using WMS.Application.Authentication.Responses;
using WMS.Infrastructure.Identity;
using WMS.Application.Authentication.Interfaces;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(UserManager<ApplicationUser> userManager, JwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }


    public async Task<LoginResponse?> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return null;
        }

        var valid = await _userManager.CheckPasswordAsync(user, password);

        if (!valid)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new LoginResponse
        {
            Role = roles.FirstOrDefault() ?? "",
            Token = _jwtTokenGenerator.GenerateToken(user, roles),
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }
}