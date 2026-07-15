using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data
{
    public static class WarehouseLocationSeeder
    {
        public static async Task<List<WarehouseLocation>> SeedAsync(WmsDbContext context)
        {
            if (context.WarehouseLocations.Any())
            {
                return context.WarehouseLocations.ToList();
            }

            var locations = new List<WarehouseLocation>
            {
                new("A-01-01"),
                new("A-01-02"),
                new("A-02-01"),
                new("A-02-02"),
                new("B-01-01"),
                new("B-01-02"),
                new("C-01-01"),
                new("C-01-02")
            };

            context.WarehouseLocations.AddRange(locations);
            await context.SaveChangesAsync();

            return locations;
        }
    }
}