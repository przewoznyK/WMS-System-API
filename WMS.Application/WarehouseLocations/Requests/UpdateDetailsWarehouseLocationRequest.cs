using System.ComponentModel.DataAnnotations;

namespace WMS.Application.Products.Request
{
    public class UpdateDetailsWarehouseLocationRequest
    {
        [Required(ErrorMessage = "The location code field cannot be empty.")]
        public string LocationCode { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}