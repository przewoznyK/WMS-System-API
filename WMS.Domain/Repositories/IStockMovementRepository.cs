using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IStockMovementRepository
    {
        Task Add(StockMovement movement);
        Task AddRangeAsync(IEnumerable<StockMovement> movements);
        Task<IEnumerable<StockMovement>> GetAllAsync();
        Task<int> GetCountTodayAsync();
    }
}