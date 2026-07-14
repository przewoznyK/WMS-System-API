using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Repositories
{
    public class SqlWarehouseLocationRepository : IWarehouseLocationRepository
    {
        private readonly WmsDbContext _wmsDbContext;

        public SqlWarehouseLocationRepository(WmsDbContext wmsDbContext)
        {
            _wmsDbContext = wmsDbContext;
        }

        public Task Add(WarehouseLocation warehouseLocation)
        {
            _wmsDbContext.WarehouseLocations.Add(warehouseLocation);
            return Task.CompletedTask;
        }

        public Task Delete(WarehouseLocation warehouseLocation)
        {
            _wmsDbContext.WarehouseLocations.Remove(warehouseLocation);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<WarehouseLocation>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _wmsDbContext.WarehouseLocations
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetLocationsAsync(CancellationToken cancellationToken)
        {
            return await _wmsDbContext.WarehouseLocations
                .Select(x => x.Code)
                .ToListAsync(cancellationToken);
        }

        public async Task<WarehouseLocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.WarehouseLocations
                .FindAsync([id], cancellationToken);
        }

        public async Task<WarehouseLocation?> GetByCodeAsync(string code, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.WarehouseLocations
                .FirstOrDefaultAsync(
                    l => l.Code == code,
                    cancellationToken);
        }

        public async Task UpdateDetailsAsync(WarehouseLocation warehouseLocation, string code, string description, CancellationToken cancellationToken)
        {
            warehouseLocation.UpdateDetails(code, description);
            await _wmsDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.WarehouseLocations
                .AnyAsync(
                    p => p.Code == code,
                    cancellationToken);
        }

        public Task<int> GetCountAsync(CancellationToken cancellationToken)
        {
            return _wmsDbContext.WarehouseLocations
                .CountAsync(cancellationToken);
        }
    }
}