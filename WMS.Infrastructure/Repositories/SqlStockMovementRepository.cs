using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Repositories
{
    public class SqlStockMovementRepository : IStockMovementRepository
    {
        private readonly WmsDbContext _wmsDbContext;

        public SqlStockMovementRepository(WmsDbContext wmsDbContext)
        {
            _wmsDbContext = wmsDbContext;
        }

        public Task Add(StockMovement movement)
        {
            _wmsDbContext.StockMovements.Add(movement);
            return Task.CompletedTask;
        }

        public async Task AddRangeAsync(IEnumerable<StockMovement> movements, CancellationToken cancellationToken)
        {
            await _wmsDbContext.StockMovements
                .AddRangeAsync(movements, cancellationToken);
        }

        public async Task<IEnumerable<StockMovement>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _wmsDbContext.StockMovements
                .ToListAsync(cancellationToken);
        }

        public Task<int> GetCountTodayAsync(CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            return _wmsDbContext.StockMovements
                .CountAsync(
                    x => x.CreatedAt >= today,
                    cancellationToken);
        }

        public async Task<IEnumerable<StockMovement>> GetBetweenDatesAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.StockMovements
                .Where(x =>
                    x.CreatedAt >= fromDate &&
                    x.CreatedAt < toDate)
                .ToListAsync(cancellationToken);
        }
    }
}