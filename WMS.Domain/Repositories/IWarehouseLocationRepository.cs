using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IWarehouseLocationRepository
    {
        Task Add(WarehouseLocation warehouseLocation);
        Task Delete(WarehouseLocation warehouseLocation);
        Task<IEnumerable<WarehouseLocation>> GetAllAsync();
        Task<IEnumerable<string>> GetLocationsAsync();
        Task<WarehouseLocation?> GetByIdAsync(Guid id);
        Task<WarehouseLocation?> GetByCodeAsync(string code);
        Task<bool> ExistsByCodeAsync(string code);
    }
}