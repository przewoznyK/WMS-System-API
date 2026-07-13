using WMS.Application.Dashboard.Responses;

namespace WMS.Client.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryResponse?> GetSummaryAsync(CancellationToken ct);
    }
}
