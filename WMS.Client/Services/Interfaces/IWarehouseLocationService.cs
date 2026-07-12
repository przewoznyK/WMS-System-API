using WMS.Application.WarehouseLocations.Dtos;

namespace WMS.Client.Services.Interfaces
{
    public interface IWarehouseLocationService
    {
        Task<IEnumerable<string>> GetCodesAsync(CancellationToken ct);
        Task<bool> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct);
    }
}
