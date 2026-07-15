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
                new("AA-01-01"),
                new("AA-01-02"),
                new("AA-02-01"),
                new("AA-02-02"),
                new("BB-01-01"),
                new("BB-01-02"),
                new("CC-01-01"),
                new("CC-01-02")
            };

            context.WarehouseLocations.AddRange(locations);
            await context.SaveChangesAsync();

            return locations;
        }
    }
}