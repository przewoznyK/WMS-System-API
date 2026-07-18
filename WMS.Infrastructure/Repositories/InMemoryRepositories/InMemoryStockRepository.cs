using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Repositories
{
    public class InMemoryStockRepository : IStockRepository
    {
        private readonly List<Stock> _stocks = new();

        public Task Add(Stock stock)
        {
            _stocks.Add(stock);
            return Task.CompletedTask;
        }

        public Task Delete(Stock stock)
        {
            _stocks.Remove(stock);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Stock>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_stocks.AsEnumerable());
        }

        public Task<IEnumerable<Stock>> GetAllByProductSkuAsync(string sku, CancellationToken cancellationToken)
        {
            var result = _stocks.Where(s => s.Product != null && s.Product.Sku == sku);
            return Task.FromResult(result);
        }

        public Task<Stock?> GetByProductIdAndLocationAsync(Guid productId, Guid locationId, CancellationToken cancellationToken)
        {
            var stock = _stocks.FirstOrDefault(s => s.ProductId == productId && s.LocationId == locationId);
            return Task.FromResult(stock);
        }

        public Task<Stock?> GetByProductSkuAndLocationCodeAsync(string sku, string locationCode, CancellationToken cancellationToken)
        {
            var stock = _stocks.FirstOrDefault(s => s.Product.Sku == sku && s.Location.Code == locationCode);
            return Task.FromResult(stock);
        }

        public Task<int> GetSumQuantityAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_stocks.Sum(s => s.Quantity));
        }

        public Task<int> GetLowStockCountAsync(int threshold, CancellationToken cancellationToken)
        {
            var result = _stocks.GroupBy(s => s.ProductId)
                                .Count(g => g.Sum(s => s.Quantity) <= threshold);
            return Task.FromResult(result);
        }

        public async Task<IEnumerable<LowStockProduct>> GetLowStockAsync(int threshold, CancellationToken cancellationToken)
        {
            var result = _stocks.GroupBy(s => new { s.ProductId, s.Product.Sku, s.Product.Name })
                                .Where(g => g.Sum(s => s.Quantity) <= threshold)
                                .Select(g => new LowStockProduct
                                {
                                    Sku = g.Key.Sku,
                                    Name = g.Key.Name,
                                    Quantity = g.Sum(s => s.Quantity)
                                })
                                .ToList();

            return await Task.FromResult(result);
        }

        public void Clear()
        {
            _stocks.Clear();
        }
    }
}