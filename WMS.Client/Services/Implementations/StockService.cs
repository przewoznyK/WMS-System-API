using WMS.Application.Stocks.Request;
using WMS.Application.Stocks.Response;
using WMS.Client.Services.Interfaces;

namespace WMS.Client.Services.Implementations
{
    public class StockService : IStockService
    {
        private readonly ApiClientService _apiClientService;

        public StockService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public Task<IEnumerable<StockResponse>?> SearchBySkuAsync(string searchTerm, CancellationToken ct)
        {
            return _apiClientService.GetAsync<IEnumerable<StockResponse>>($"api/stocks/by-product-sku/{searchTerm}", ct);
        }

        public Task<IEnumerable<StockResponse>> GetSummaryAsync(CancellationToken ct)
        {
            return _apiClientService.GetListAsync<StockResponse>("api/stocks/summary", ct);
        }

        public Task<bool> MoveAsync(MoveStockRequest request, CancellationToken ct)
        {
            return _apiClientService.PostAsync("api/stocks/move", request, ct);
        }

        public Task<bool> IssueAsync(IssueStockRequest request, CancellationToken ct)
        {
            return _apiClientService.PostAsync("api/stocks/issue", request, ct);
        }

        public Task<bool> ReceiveAsync(ReceiveStockRequest request, CancellationToken ct)
        {
            return _apiClientService.PostAsync("api/stocks/receive", request, ct);
        }
    }
}
