using WMS.Application.StockMovements.Response;
using WMS.Application.StockMovements.Responses;

namespace WMS.Client.Services.Interfaces
{
    public interface IStockMovementService
    {
        Task<IEnumerable<StockMovementResponse>> GetSummaryAsync(CancellationToken ct);
        Task<IEnumerable<StockMovementChartResponse>> GetChartAsync(int days, CancellationToken ct);
    }
}
