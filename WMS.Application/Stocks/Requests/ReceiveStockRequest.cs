using System.ComponentModel.DataAnnotations;

namespace WMS.Application.Stocks.Request
{
    public class ReceiveStockRequest
    {
        [Required(ErrorMessage = "Product SKU is required.")]
        public string? ProductSku { get; set; }

        [Required(ErrorMessage = "Location code is required.")]
        public string? LocationCode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be minimum 1.")]
        public int Quantity { get; set; } = 1;
    }
}