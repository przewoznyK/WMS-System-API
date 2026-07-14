using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IWarehouseLocationRepository
    {
        Task Add(WarehouseLocation warehouseLocation);
        Task Delete(WarehouseLocation warehouseLocation);
        Task<IEnumerable<WarehouseLocation>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetLocationsAsync(CancellationToken cancellationToken);
        Task<WarehouseLocation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<WarehouseLocation?> GetByCodeAsync(string code, CancellationToken cancellationToken);
        Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken);
        Task<int> GetCountAsync(CancellationToken cancellationToken);
    }
}