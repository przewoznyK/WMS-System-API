using System.ComponentModel.DataAnnotations;

namespace WMS.Application.Products.Dtos
{
    public class AddProductDto
    {
        [Required(ErrorMessage = "The Sku field cannot be empty.")]
        public string Sku { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Product name field cannot be empty.")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}