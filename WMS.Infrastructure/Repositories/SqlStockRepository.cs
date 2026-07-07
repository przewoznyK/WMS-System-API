using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Repositories
{
    public class SqlStockRepository : IStockRepository
    {
        private readonly WmsDbContext _wmsDbContext;

        public SqlStockRepository(WmsDbContext wmsDbContext)
        {
            _wmsDbContext = wmsDbContext;
        }

        public async Task AddAsync(Stock stock)
        {
            await _wmsDbContext.Stocks.AddAsync(stock);
        }

        public async Task<IEnumerable<Stock>> GetAllAsync()
        {
            return await _wmsDbContext.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByProductAndLocationAsync(Guid productId, Guid locationId)
        {
            return await _wmsDbContext.Stocks
                .FirstOrDefaultAsync(s => s.ProductId == productId && s.LocationId == locationId);
        }
    }
}