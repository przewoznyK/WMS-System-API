using Microsoft.AspNetCore.Identity;
using WMS.Domain.Enums;

namespace WMS.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        UserRoleType[] roles =
        {
            UserRoleType.Worker,
            UserRoleType.Manager,
            UserRoleType.Admin
        };

        foreach (var role in roles)
        {
            var roleName = role.ToString();

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(roleName));
            }
        }


        var email = "manager@wms.com";

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };


            await userManager.CreateAsync(user,"Password123!");
            await userManager.AddToRoleAsync(user,"Manager");
        }

        if (await userManager.FindByEmailAsync("worker@wms.com") == null)
        {
            var worker = new ApplicationUser
            {
                UserName = "worker@wms.com",
                Email = "worker@wms.com"
            };

            await userManager.CreateAsync(worker, "Password123!");
            await userManager.AddToRoleAsync(worker, "Worker");
        }
    }
}