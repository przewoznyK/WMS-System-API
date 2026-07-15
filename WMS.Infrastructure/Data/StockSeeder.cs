using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data
{
    public static class StockSeeder
    {
        public static async Task<List<Stock>> SeedAsync(WmsDbContext context, List<Product> products, List<WarehouseLocation> locations)
        {
            if (context.Stocks.Any())
            {
                return context.Stocks.ToList();
            }

            var stocks = new List<Stock>
            {
                new(products[0], locations[0], 120),
                new(products[1], locations[1], 45),
                new(products[2], locations[0], 15),
                new(products[3], locations[1], 8),
                new(products[4], locations[2], 80),
                new(products[5], locations[2], 60),
                new(products[6], locations[4], 12),
                new(products[7], locations[4], 5),
                new(products[8], locations[3], 200),
                new(products[9], locations[3], 30),
                new(products[10], locations[6], 500),
                new(products[11], locations[7], 150)
            };

            context.Stocks.AddRange(stocks);
            await context.SaveChangesAsync();

            return stocks;
        }
    }
}