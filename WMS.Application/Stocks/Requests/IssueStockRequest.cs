using System.ComponentModel.DataAnnotations;
using WMS.Domain.Enums;

namespace WMS.Application.Stocks.Request
{
    public class IssueStockRequest
    {
        [Required(ErrorMessage = "Product is required.")]
        public string ProductSku { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Source Location Code field is required.")]
        [RegularExpression(@"^[A-Z]{2}-\d{2}-\d{2}$", ErrorMessage = "The format must comply with the pattern: AA-00-00.")]
        public string LocationCode
        {
            get => _sourceLocationCode;
            set => _sourceLocationCode = value?.ToUpper() ?? string.Empty;
        }
        private string _sourceLocationCode = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        public IssueType IssueType { get; set; } = IssueType.SalesOrder;

        [MaxLength(50)]
        public string ReferenceNumber { get; set; } = string.Empty;
    }
}