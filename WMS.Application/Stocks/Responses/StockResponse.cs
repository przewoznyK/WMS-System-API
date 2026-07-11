namespace WMS.Application.Stocks.Response
{
    public class StockResponse
    {
        public string ProductSku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}