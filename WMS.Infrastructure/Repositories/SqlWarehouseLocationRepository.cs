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

        public async Task<IEnumerable<WarehouseLocation>> GetAllAsync()
        {
            return await _wmsDbContext.WarehouseLocations.ToListAsync();
        }


        public async Task<IEnumerable<string>> GetLocationsAsync()
        {
            return await _wmsDbContext.WarehouseLocations
              .Select(x => x.Code)
              .ToListAsync();
        }

        public async Task<WarehouseLocation?> GetByIdAsync(Guid id)
        {
            return await _wmsDbContext.WarehouseLocations.FindAsync(id);
        }

        public async Task<WarehouseLocation?> GetByCodeAsync(string code)
        {
            return await _wmsDbContext.WarehouseLocations.FirstOrDefaultAsync(l => l.Code == code);
        }

        public async Task UpdateDetailsAsync(WarehouseLocation warehouseLocation, string code, string description)
        {
            warehouseLocation.UpdateDetails(code, description);
            await _wmsDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _wmsDbContext.WarehouseLocations
                .AnyAsync(p => p.Code == code);
        }

        public Task<int> GetCountAsync()
        {
            return _wmsDbContext.WarehouseLocations.CountAsync();
        }
    }
}
