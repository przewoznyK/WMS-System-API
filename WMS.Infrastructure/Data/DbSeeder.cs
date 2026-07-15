using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WMS.Infrastructure.Identity;

namespace WMS.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<WmsDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await IdentitySeeder.SeedAsync(userManager, roleManager);

            var manager = await userManager.FindByEmailAsync("manager@wms.com");
            var worker = await userManager.FindByEmailAsync("worker@wms.com");

            if (manager == null || worker == null)
            {
                return;
            }

            var products = await ProductSeeder.SeedAsync(context);
            var locations = await WarehouseLocationSeeder.SeedAsync(context);
            var stocks =await StockSeeder.SeedAsync(context, products, locations);
            await StockMovementSeeder.SeedAsync(context, stocks, manager.Id, worker.Id);
        }
    }
}