using WMS.Domain.Exceptions;

namespace WMS.Domain.Entities
{
    public class Stock
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid LocationId { get; private set; }
        public int Quantity { get; private set; }

        public Product Product { get; private set; }
        public WarehouseLocation Location { get; private set; }

        public Stock()
        {

        }

        public Stock(Product product, WarehouseLocation location, int quantity)
        {
            if (product == null)
            {
                throw new WmsNullOrEmptyException(nameof(product));
            }

            if (location == null)
            {
                throw new WmsNullOrEmptyException(nameof(location));
            }

            if (quantity < 0)
            {
                throw new WmsBusinessRuleException("Stock quantity cannot be negative.");
            }

            Id = Guid.NewGuid();
            ProductId = product.Id;
            LocationId = location.Id;
            Product = product;
            Location = location;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int value)
        {
            if (value <= 0)
            {
                throw new WmsBusinessRuleException("Increase value must be greater than zero.");
            }

            Quantity += value;
        }

        public void DecreaseQuantity(int value)
        {
            if (value <= 0)
            {
                throw new WmsBusinessRuleException("Decrease value must be greater than zero.");
            }

            if (Quantity < value)
            {
                throw new WmsBusinessRuleException("Can't take more quantity than is in the stock.");
            }

            Quantity -= value;
        }
    }
}