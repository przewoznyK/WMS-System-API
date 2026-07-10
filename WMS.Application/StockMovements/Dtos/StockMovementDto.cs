using WMS.Domain.Enums;

namespace WMS.Application.StockMovements.Dtos
{
    public class StockMovementDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }

        public string ProductSku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;

        public OperationType OperationType { get; private set; }
        public int QuantityChange { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}