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

        public async Task<IEnumerable<Stock>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Stocks
                .Include(s => s.Product)
                .Include(s => s.Location)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Stock>> GetAllByProductSkuAsync(string sku, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Stocks
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Where(s => s.Product.Sku == sku)
                .ToListAsync(cancellationToken);
        }

        public async Task<Stock?> GetByProductIdAndLocationAsync(Guid productId, Guid locationId, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Stocks
                .FirstOrDefaultAsync(
                    s => s.ProductId == productId &&
                         s.LocationId == locationId,
                    cancellationToken);
        }

        public async Task<Stock?> GetByProductSkuAndLocationCodeAsync(string sku, string locationCode, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Stocks
                .Include(s => s.Product)
                .Include(s => s.Location)
                .FirstOrDefaultAsync(
                    s => s.Product.Sku == sku &&
                         s.Location.Code == locationCode,
                    cancellationToken);
        }

        public Task<int> GetSumQuantityAsync(CancellationToken cancellationToken)
        {
            return _wmsDbContext.Stocks
                .SumAsync(
                    x => x.Quantity,
                    cancellationToken);
        }

        public Task<int> GetLowStockCountAsync(int threshold, CancellationToken cancellationToken)
        {
            return _wmsDbContext.Stocks
                .GroupBy(x => x.ProductId)
                .Where(x => x.Sum(s => s.Quantity) <= threshold)
                .CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<LowStockProduct>> GetLowStockAsync(int threshold, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Stocks
                .Include(x => x.Product)
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.Product.Sku,
                    x.Product.Name
                })
                .Where(x => x.Sum(s => s.Quantity) <= threshold)
                .Select(x => new LowStockProduct
                {
                    Sku = x.Key.Sku,
                    Name = x.Key.Name,
                    Quantity = x.Sum(s => s.Quantity)
                })
                .ToListAsync(cancellationToken);
        }
    }
}