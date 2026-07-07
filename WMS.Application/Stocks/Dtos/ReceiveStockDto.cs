namespace WMS.Application.Stocks.Dtos
{
    public class ReceiveStockDto
    {
        public string Sku { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
