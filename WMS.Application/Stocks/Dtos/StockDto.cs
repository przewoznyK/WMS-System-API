namespace WMS.Application.Stocks.Dtos
{
    public class StockDto
    {
        public string ProductSku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}