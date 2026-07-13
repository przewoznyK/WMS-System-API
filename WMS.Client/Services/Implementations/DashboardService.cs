using WMS.Application.Dashboard.Responses;
using WMS.Client.Services.Interfaces;

namespace WMS.Client.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly ApiClientService _apiClientService;

        public DashboardService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public Task<DashboardSummaryResponse?> GetSummaryAsync(
            CancellationToken ct)
        {
            return _apiClientService.GetAsync<DashboardSummaryResponse>("api/dashboard/summary", ct);
        }
    }
}
