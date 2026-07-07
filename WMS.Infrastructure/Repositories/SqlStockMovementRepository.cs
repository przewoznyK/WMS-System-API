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

        public async Task AddAsync(StockMovement movement)
        {
            await _wmsDbContext.StockMovements.AddAsync(movement);
        }

        public async Task AddRangeAsync(IEnumerable<StockMovement> movements)
        {
            await _wmsDbContext.StockMovements.AddRangeAsync(movements);
        }

        public async Task<IEnumerable<StockMovement>> GetAllAsync()
        {
            return await _wmsDbContext.StockMovements.ToListAsync();
        }
    }
}