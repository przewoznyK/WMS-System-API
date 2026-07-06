using WMS.Domain.Exceptions;

namespace WMS.Domain.Entities
{
    public class Stock
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid LocationId { get; private set; }
        public int Quantity { get; private set; }

        public Stock(Guid productId, Guid locationId, int quantity)
        {
            if (quantity < 0)
            {
                throw new WmsBusinessRuleException("Stock quantity cannot be negative.");
            }

            Id = Guid.NewGuid();
            ProductId = productId;
            LocationId = locationId;
            Quantity = quantity;
        }
    }
}