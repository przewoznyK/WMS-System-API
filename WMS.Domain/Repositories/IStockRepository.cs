using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IStockRepository
    {
        Task Add(Stock stock);
        Task Delete(Stock stock);
        Task<IEnumerable<Stock>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Stock>> GetAllByProductSkuAsync(string sku, CancellationToken cancellationToken);
        Task<Stock?> GetByProductIdAndLocationAsync(Guid productId, Guid locationId, CancellationToken cancellationToken);
        Task<Stock?> GetByProductSkuAndLocationCodeAsync(string productSku, string locationCode, CancellationToken cancellationToken);
        Task<int> GetSumQuantityAsync(CancellationToken cancellationToken);
        Task<int> GetLowStockCountAsync(int threshold, CancellationToken cancellationToken);
        Task<IEnumerable<LowStockProduct>> GetLowStockAsync(int threshold, CancellationToken cancellationToken);
    }
}