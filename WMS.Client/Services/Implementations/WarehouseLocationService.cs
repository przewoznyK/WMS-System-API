using WMS.Application.Stocks.Response;
using WMS.Application.WarehouseLocations.Dtos;
using WMS.Client.Services.Interfaces;

namespace WMS.Client.Services.Implementations
{
    public class WarehouseLocationService : IWarehouseLocationService
    {
        private readonly ApiClientService _apiClientService;

        public WarehouseLocationService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public Task<IEnumerable<string>> GetCodesAsync(CancellationToken ct)
        {
            return _apiClientService.GetListAsync<string>("api/warehouseLocations/location-codes", ct);
        }

        public Task<bool> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct)
        {
            return _apiClientService.PostAsync("api/warehouseLocations/create", request, ct);
        }
    }
}
