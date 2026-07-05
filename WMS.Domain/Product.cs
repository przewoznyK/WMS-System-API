using WMS.Domain.Exceptions;

namespace WMS.Domain
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Sku { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Product(string sku, string name, string description = "")
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new InvalidProductDataException("SKU cannot be null");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidProductDataException("Name cannot be null");
            }

            Id = Guid.NewGuid();
            Sku = sku;
            Name = name;
            Description = description;
        }

        public void UpdateDetails(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidProductDataException("Name cannot be null");
            }

            Name = name;
            Description = description;
        }
    }
}