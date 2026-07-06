using WMS.Domain.Exceptions;

namespace WMS.Domain.Entities
{
    public class StockMovement
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid LocationId { get; private set; }
        public int QuantityChange { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public StockMovement(Guid productId, Guid locationId, int quantityChange)
        {
            if (quantityChange == 0)
            {
                throw new InvalidMovementQuantityException("Stock movement quantity cannot be zero.");
            }

            Id = Guid.NewGuid();
            ProductId = productId;
            LocationId = locationId;
            QuantityChange = quantityChange;
            CreatedAt = DateTime.UtcNow;
        }
    }
}