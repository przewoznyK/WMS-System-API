using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data
{
    public static class ProductSeeder
    {
        public static async Task<List<Product>> SeedAsync(WmsDbContext context)
        {
            if (context.Products.Any())
            {
                return context.Products.ToList();
            }

            var products = new List<Product>
            {
                new("LAP-001", "Lenovo ThinkPad T14"),
                new("LAP-002", "Dell Latitude 5440"),
                new("MON-001", "Dell UltraSharp 27"),
                new("MON-002", "LG UltraWide 34"),
                new("MOU-001", "Logitech MX Master 3"),
                new("KEY-001", "Logitech Mechanical Keyboard"),
                new("SCN-001", "Zebra Barcode Scanner"),
                new("PRN-001", "Zebra Label Printer"),
                new("CAB-001", "USB-C Cable"),
                new("CAB-002", "HDMI Cable"),
                new("BOX-001", "Shipping Box Small"),
                new("BOX-002", "Shipping Box Large")
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            return products;
        }
    }
}