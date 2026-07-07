using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IStockRepository
    {
        public Task AddAsync(Stock stock);
        public Task<IEnumerable<Stock>> GetAllAsync();
        public Task<Stock?> GetByProductAndLocationAsync(Guid productId, Guid locationId);
    }
}