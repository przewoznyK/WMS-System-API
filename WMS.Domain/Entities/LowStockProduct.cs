namespace WMS.Domain.Entities
{
    public class LowStockProduct
    {
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
