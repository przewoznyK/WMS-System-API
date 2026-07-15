using WMS.Domain.Enums;
using WMS.Domain.Exceptions;

namespace WMS.Domain.Entities
{
    public class StockMovement
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid LocationId { get; private set; }

        public string ProductSku { get; private set; }
        public string ProductName { get; private set; }
        public string LocationCode { get; private set; }
        public OperationType OperationType { get; private set; }
        public int QuantityChange { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public string CreatedByUserId { get; private set; }

        public IssueType? IssueType { get; set; }
        public string? ReferenceNumber { get; set; }

        private StockMovement() 
        {
            ProductSku = null!;
            ProductName = null!;
            LocationCode = null!;
            CreatedByUserId = null!;
        }

        public StockMovement(Stock stock, OperationType operationType, int quantityChange, string createdByUserId, IssueType? issueType = null, string? referenceNumber = null)
        {
            if (quantityChange == 0)
            {
                throw new WmsBusinessRuleException("Stock movement quantity cannot be zero.");
            }

            Id = Guid.NewGuid();
            ProductId = stock.ProductId;
            LocationId = stock.LocationId;
            ProductSku = stock.Product.Sku;
            ProductName = stock.Product.Name;
            LocationCode = stock.Location.Code;
            OperationType = operationType;
            QuantityChange = quantityChange;
            CreatedAt = DateTime.UtcNow;
            CreatedByUserId = createdByUserId;
            IssueType = issueType;
            ReferenceNumber = referenceNumber;
        }

        public void SetCreatedAt(DateTime date)
        {
            CreatedAt = date;
        }
    }
}