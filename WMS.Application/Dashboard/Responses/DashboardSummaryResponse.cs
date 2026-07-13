namespace WMS.Application.Dashboard.Responses
{
    public class DashboardSummaryResponse
    {
        public int ProductCount { get; init; }
        public int LocationCount { get; init; }
        public int StockQuantity { get; init; }
        public int TodayMovements { get; init; }
    }
}