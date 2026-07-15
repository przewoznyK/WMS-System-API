namespace WMS.Domain.Data
{
    public class StockMovementChartData
    {
        public DateTime Date { get; set; }
        public int ReceiveCount { get; set; }
        public int IssueCount { get; set; }
        public int TransferCount { get; set; }
    }
}
