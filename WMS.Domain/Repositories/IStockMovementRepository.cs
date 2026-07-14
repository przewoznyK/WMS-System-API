using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IStockMovementRepository
    {
        Task Add(StockMovement movement);
        Task AddRangeAsync(IEnumerable<StockMovement> movements, CancellationToken cancellationToken);
        Task<IEnumerable<StockMovement>> GetAllAsync(CancellationToken cancellationToken);
        Task<int> GetCountTodayAsync(CancellationToken cancellationToken);
        Task<IEnumerable<StockMovement>> GetBetweenDatesAsync(DateTime fromData, DateTime toData, CancellationToken cancellationToken);
    }
}