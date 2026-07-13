using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IStockRepository
    {
        Task Add(Stock stock);
        Task Delete(Stock stock);
        Task<IEnumerable<Stock>> GetAllAsync();
        Task<IEnumerable<Stock>> GetAllByProductSkuAsync(string sku);
        Task<Stock?> GetByProductIdAndLocationAsync(Guid productId, Guid locationId);
        Task<Stock?> GetByProductSkuAndLocationCodeAsync(string productSku, string locationCode);
        Task<int> GetSumQuantityAsync();
    }
}