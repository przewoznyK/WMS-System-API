using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IStockMovementRepository
    {
        public Task AddAsync(StockMovement movement);
        public Task AddRangeAsync(IEnumerable<StockMovement> movements);
        public Task<IEnumerable<StockMovement>> GetAllAsync();

    }
}
