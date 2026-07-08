using System.ComponentModel.DataAnnotations;

namespace WMS.Application.Stocks.Dtos
{
    public class ReceiveStockDto
    {
        [Required(ErrorMessage = "The Product name field cannot be empty.")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location code is required.")]
        public string LocationCode { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be minimum 1.")]
        public int Quantity { get; set; } = 1;
    }
}