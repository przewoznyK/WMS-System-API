using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WMS.Infrastructure.Identity;

public class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;


    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string GenerateToken(
       ApplicationUser user,
       IEnumerable<string> roles)
    {
        var claims = new List<Claim>
{
    new Claim(
        ClaimTypes.NameIdentifier,
        user.Id),

    new Claim(
        ClaimTypes.Email,
        user.Email!),

    new Claim(
        JwtRegisteredClaimNames.Email,
        user.Email!)
};


        foreach (var role in roles)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    role));
        }


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));


        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(
                    _configuration["Jwt:ExpireMinutes"]!)),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}