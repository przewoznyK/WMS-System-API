using System.ComponentModel.DataAnnotations;

namespace WMS.Application.WarehouseLocations.Dtos
{
    public class AddLocationDto
    {
        [Required(ErrorMessage = "The Location Code field is required.")]
        [RegularExpression(@"^[A-Z]{2}-\d{2}-\d{2}$", ErrorMessage = "The format must comply with the pattern: AA-00-00.")]
        public string Code
        {
            get => _code;
            set => _code = value?.ToUpper() ?? string.Empty;
        }
        public string Description { get; set; } = string.Empty;
        private string _code = string.Empty;
    }
}