using System.ComponentModel.DataAnnotations;

namespace WMS.Application.Stocks.Request
{
    public class MoveStockRequest
    {

        [Required(ErrorMessage = "The Source Location Code field is required.")]
        [RegularExpression(@"^[A-Z]{2}-\d{2}-\d{2}$", ErrorMessage = "The format must comply with the pattern: AA-00-00.")]
        public string SourceLocationCode
        {
            get => _sourceLocationCode;
            set => _sourceLocationCode = value?.ToUpper() ?? string.Empty;
        }

        private string _sourceLocationCode = string.Empty;

        [Required(ErrorMessage = "The Destination Location Code field is required.")]
        [RegularExpression(@"^[A-Z]{2}-\d{2}-\d{2}$", ErrorMessage = "The format must comply with the pattern: AA-00-00.")]
        public string DestinationLocationCode
        {
            get => _destinationLocationCode;
            set => _destinationLocationCode = value?.ToUpper() ?? string.Empty;
        }

        private string _destinationLocationCode = string.Empty;

        public string ProductSku { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}