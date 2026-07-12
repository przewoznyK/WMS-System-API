using WMS.Application.StockMovements.Response;
using WMS.Application.Stocks.Request;
using WMS.Application.Stocks.Response;
using WMS.Client.Services.Interfaces;

namespace WMS.Client.Services.Implementations
{
    public class StockMovementService : IStockMovementService
    {
        private readonly ApiClientService _apiClientService;

        public StockMovementService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public Task<IEnumerable<StockMovementResponse>> GetSummaryAsync(CancellationToken ct)
        {
            return _apiClientService.GetListAsync<StockMovementResponse>("api/stockmovements/summary", ct);
        }
    }
}
