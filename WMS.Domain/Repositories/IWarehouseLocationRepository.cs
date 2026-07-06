using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IWarehouseLocationRepository
    {
        public Task AddAsync(WarehouseLocation warehouseLocation);
        public Task DeleteAsync(WarehouseLocation warehouseLocation);
        public Task<IEnumerable<WarehouseLocation>> GetAllAsync();
        public Task<WarehouseLocation?> GetWarehouseByIdAsync(Guid id);
        public Task UpdateDetailsAsync(WarehouseLocation warehouseLocation, string code, string description);
        public Task<bool> ExistsByCodeAsync(string code);
    }
}