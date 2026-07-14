namespace WMS.Application.StockMovements.Responses
{
    public class MovementChartResponse
    {
        public DateTime Date { get; set; }
        public int Receive { get; set; }
        public int Issue { get; set; }
        public int Transfer { get; set; }
    }
}