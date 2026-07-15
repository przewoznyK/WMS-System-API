using WMS.Domain.Enums;

namespace WMS.Application.StockMovements.Response
{
    public class StockMovementResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }

        public string ProductSku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;

        public OperationType OperationType { get; set; }
        public int QuantityChange { get; set; }
        public DateTime CreatedAt { get; set; }

        public string CreatedByUserId { get; set; } = null!;
        public string CreatedByName { get; set; } = string.Empty;

        public IssueType? IssueType { get; set; }
        public string? ReferenceNumber { get; set; }

    }
}