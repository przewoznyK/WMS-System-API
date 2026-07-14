namespace WMS.Application.Stocks.Responses
{
    public class LowStockProductResponse
    {
        public string ProductSku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
