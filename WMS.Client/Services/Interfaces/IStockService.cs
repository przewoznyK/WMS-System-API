using WMS.Application.StockMovements.Response;
using WMS.Application.Stocks.Request;
using WMS.Application.Stocks.Response;
using WMS.Application.Stocks.Responses;

namespace WMS.Client.Services.Interfaces
{
    public interface IStockService
    {
        Task<IEnumerable<StockResponse>?> SearchBySkuAsync(string searchTerm, CancellationToken ct);
        Task<IEnumerable<StockResponse>> GetSummaryAsync(CancellationToken ct);
        Task<bool> MoveAsync(MoveStockRequest request, CancellationToken ct);
        Task<bool> IssueAsync(IssueStockRequest request, CancellationToken ct);
        Task<bool> ReceiveAsync(ReceiveStockRequest request, CancellationToken ct);
        Task<IEnumerable<LowStockProductResponse>> GetLowStockProductsAsync(CancellationToken ct);
    }
}
