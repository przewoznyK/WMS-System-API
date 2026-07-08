using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IWarehouseLocationRepository
    {
        public Task AddAsync(WarehouseLocation warehouseLocation);
        public Task DeleteAsync(WarehouseLocation warehouseLocation);
        public Task<IEnumerable<WarehouseLocation>> GetAllAsync();
        public Task<IEnumerable<string>> GetLocationsAsync();
        public Task<WarehouseLocation?> GetByIdAsync(Guid id);
        public Task<WarehouseLocation?> GetByCodeAsync(string code);
        public Task UpdateDetailsAsync(WarehouseLocation warehouseLocation, string code, string description);
        public Task<bool> ExistsByCodeAsync(string code);
    }
}