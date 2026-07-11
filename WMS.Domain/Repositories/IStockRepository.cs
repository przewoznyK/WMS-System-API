using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IStockRepository
    {
        public Task AddAsync(Stock stock);
        public Task DeleteAsync(Stock stock);
        public Task<IEnumerable<Stock>> GetAllAsync();
        public Task<IEnumerable<Stock>> GetAllByProductSkuAsync(string sku);
        public Task<Stock?> GetByProductIdAndLocationAsync(Guid productId, Guid locationId);
        public Task<Stock?> GetByProductSkuAndLocationCodeAsync(string sku, string locationSku);
    }
}