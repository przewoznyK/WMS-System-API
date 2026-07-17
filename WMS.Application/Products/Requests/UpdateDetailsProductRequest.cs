using System.ComponentModel.DataAnnotations;

namespace WMS.Application.Products.Request
{
    public class UpdateDetailsProductRequest
    {
        [Required(ErrorMessage = "The product name field cannot be empty.")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}