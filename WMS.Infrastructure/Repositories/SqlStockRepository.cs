using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Repositories
{
    public class SqlStockRepository : IStockRepository
    {
        private readonly WmsDbContext _wmsDbContext;

        public SqlStockRepository(WmsDbContext wmsDbContext)
        {
            _wmsDbContext = wmsDbContext;
        }

        public Task Add(Stock stock)
        {
            _wmsDbContext.Stocks.Add(stock);
            return Task.CompletedTask;
        }

        public Task Delete(Stock stock)
        {
            _wmsDbContext.Stocks.Remove(stock);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Stock>> GetAllAsync()
        {
            return await _wmsDbContext.Stocks
             .Include(s => s.Product)
             .Include(s => s.Location)
             .ToListAsync();
        }

        public async Task<IEnumerable<Stock>> GetAllByProductSkuAsync(string sku)
        {
            return await _wmsDbContext.Stocks
             .Include(s => s.Product)
             .Include(s => s.Location)
             .Where(s => s.Product.Sku == sku)
             .ToListAsync();
        }

        public async Task<Stock?> GetByProductIdAndLocationAsync(Guid productId, Guid locationId)
        {
            return await _wmsDbContext.Stocks
                .FirstOrDefaultAsync(s => s.ProductId == productId && s.LocationId == locationId);
        }

        public async Task<Stock?> GetByProductSkuAndLocationCodeAsync(string sku, string locationCode)
        {
            return await _wmsDbContext.Stocks
                    .Include(s => s.Product) 
                    .Include(s => s.Location)
                    .FirstOrDefaultAsync(s => s.Product.Sku == sku && s.Location.Code == locationCode);
        }
        
    }
}