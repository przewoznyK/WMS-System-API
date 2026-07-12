using WMS.Application.StockMovements.Response;

namespace WMS.Client.Services.Interfaces
{
    public interface IStockMovementService
    {
        Task<IEnumerable<StockMovementResponse>> GetSummaryAsync(CancellationToken ct);
    }
}
